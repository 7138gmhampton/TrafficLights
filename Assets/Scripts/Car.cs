using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST,
        NONE
    }

    public float moveTime;
    public LayerMask blockingLayer;

    private Rigidbody2D carRigidbody;
    private BoxCollider2D boxCollider;
    private float inverseMoveTime;
    private Direction driveDirection;
    private Direction turnDirection;
    private bool moving;
    private bool turning;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        inverseMoveTime = 1f / moveTime;
        driveDirection = Direction.NONE;
    }

    private void Update()
    {
        if (!moving) moveCar();
    }

    private void moveCar()
    {
        switch (driveDirection) {
            case Direction.NORTH: AttemptMove(0, 1); break;
            case Direction.EAST: AttemptMove(1, 0); break;
            case Direction.SOUTH: AttemptMove(0, -1); break;
            case Direction.WEST: AttemptMove(-1, 0); break;
            case Direction.NONE: break;
        }
    }

    private void AttemptMove(int deltaX, int deltaY)
    {
        Vector2 start = transform.position;
        var end = start + new Vector2(deltaX, deltaY);

        boxCollider.enabled = false;
        var blocked = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (blocked.transform == null)
            StartCoroutine(smoothMovement(end));
    }

    private IEnumerator smoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            moving = true;
            carRigidbody.MovePosition(Vector3.MoveTowards(carRigidbody.position, end, 
                inverseMoveTime * Time.deltaTime));
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        moving = false;
    }

    private Direction pickAnotherDirection(Direction previousDirection)
    {
        Direction anotherDirection;

        do {
            anotherDirection = (Direction)Random.Range(0, 4);
        } while (anotherDirection == getOpposite(previousDirection));

        return anotherDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag) {
            case "Road": travelAlongRoad(collision); break;
            case "Junction": traverseJunction(collision); break;
            case "Despawner": despawn(); break;
        }
    }

    private void despawn()
    {
        --transform.parent.gameObject.GetComponent<CarManager>().NoOfCars;

        Destroy(gameObject);
    }

    private void traverseJunction(Collider2D collision)
    {
        if (!turning) {
            turnDirection = pickAnotherDirection(driveDirection);
            turning = true;
        }

        navigateJunction(determineCorner(collision.transform.eulerAngles.z));
    }

    private void travelAlongRoad(Collider2D collision)
    {
        turning = false;

        navigateRoad(collision.transform.rotation.eulerAngles.z);
    }

    private void navigateRoad(float roadDirection)
    {
        switch (roadDirection) {
            case float d when d > -45 && d < 45: setDriveDirection(Direction.NORTH); break;
            case float d when d > 45 && d < 135: setDriveDirection(Direction.WEST); break;
            case float d when d > 135 && d < 225: setDriveDirection(Direction.SOUTH); break;
            case float d when d > -135 && d < -45: setDriveDirection(Direction.EAST); break;
        }
    }

    private void navigateJunction(Corner corner)
    {
        switch (corner) {
            case Corner.TOP_LEFT:
                if (turnDirection == Direction.NORTH) setDriveDirection(Direction.NORTH);
                else setDriveDirection(Direction.EAST);
                break;
            case Corner.TOP_RIGHT:
                if (turnDirection == Direction.EAST) setDriveDirection(Direction.EAST);
                else setDriveDirection(Direction.SOUTH);
                break;
            case Corner.BOTTOM_RIGHT:
                if (turnDirection == Direction.SOUTH) setDriveDirection(Direction.SOUTH);
                else setDriveDirection(Direction.WEST);
                break;
            case Corner.BOTTOM_LEFT:
                if (turnDirection == Direction.WEST) setDriveDirection(Direction.WEST);
                else setDriveDirection(Direction.NORTH);
                break;
        }
    }

    private Corner determineCorner(float cornerRotation)
    {
        switch (cornerRotation) {
            case float r when r > -45 && r < 45: return Corner.BOTTOM_LEFT;
            case float r when r > 45 && r < 135: return Corner.BOTTOM_RIGHT;
            case float r when r > 135 && r < 225: return Corner.TOP_RIGHT;
            case float r when r > 225 && r < 315: return Corner.TOP_LEFT;
            default: return Corner.ERROR;
        }
    }

    private Direction getOpposite(Direction directionForward)
    {
        var opposites = new Dictionary<Direction, Direction>() 
        {
            { Direction.NORTH, Direction.SOUTH },
            { Direction.EAST, Direction.WEST },
            { Direction.SOUTH, Direction.NORTH },
            { Direction.WEST, Direction.EAST }
        };

        return opposites[directionForward];
    }

    private void setDriveDirection(Direction direction)
    {
        if (direction != driveDirection)
            switch (direction) {
                case Direction.EAST: transform.rotation = Quaternion.Euler(0, 0, -90); break;
                case Direction.NORTH: transform.rotation = Quaternion.Euler(0, 0, 0); break;
                case Direction.SOUTH: transform.rotation = Quaternion.Euler(0, 0, 0180); break;
                case Direction.WEST: transform.rotation = Quaternion.Euler(0, 0, 90); break;
            }

        driveDirection = direction;
    }
}
