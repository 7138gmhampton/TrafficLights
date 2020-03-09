using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveTime;
    public LayerMask blockingLayer;
    public LayerMask traversibleLayer;

    private Rigidbody2D carRigidbody;
    private BoxCollider2D boxCollider;
    private float inverseMoveTime;
    private Direction driveDirection;
    private Direction turnDirection;
    private bool turning;
    private float lifetime;
    private float reportTime;

    public bool Moving { get; private set; } = false;
    public Vector2 NextMovement { get; private set; }
    public float TimeSinceLastMove { get; private set; }
    public float UnacceptableWaitTime { get; set; }

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        inverseMoveTime = 1f / moveTime;
        driveDirection = Direction.NONE;
        lifetime = 0f;
        TimeSinceLastMove = 0f;
        reportTime = 0f;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        TimeSinceLastMove += Time.deltaTime;
        reportTime += Time.deltaTime;

        if (reportTime > UnacceptableWaitTime) {
            reportWaitTooLong();
        }
    }

    private void reportWaitTooLong()
    {
        FindObjectOfType<DispatcherAgent>().SendMessage("unacceptableWait");
        reportTime = 0f;

        Debug.Log(transform.position);
        Debug.Log("Before: " + driveDirection + " & " + turnDirection);
        if (turning) immediateTurnOff();
        else travelAlongRoad(Physics2D.OverlapPoint(transform.position, traversibleLayer));
        Debug.Log(driveDirection + " & " + turnDirection);
    }

    public void moveCar()
    {
        switch (driveDirection) {
            case Direction.EAST: setNextMovement(1, 0); break;
            case Direction.NORTH: setNextMovement(0, 1); break;
            case Direction.SOUTH: setNextMovement(0, -1); break;
            case Direction.WEST: setNextMovement(-1, 0); break;
            case Direction.NONE: setNextMovement(0, 0); break;
        }
    }

    private bool isSpaceInFrontClear(Vector2 start, Vector2 end)
    {
        var realCollisions = new List<RaycastHit2D>();

        foreach (var hit in Physics2D.LinecastAll(start, end, blockingLayer))
            if (hit.collider != boxCollider)
                realCollisions.Add(hit);

        return realCollisions.Count == 0 ? true : false;
    }

    public void setNextMovement(int deltaX, int deltaY)
    {
        Vector2 start = transform.position;
        var end = start + new Vector2(deltaX, deltaY);

        NextMovement = isSpaceInFrontClear(start, end) ? end : start;
    }

    public void doMovement()
    {
        if (NextMovement != (Vector2)transform.position) {
            TimeSinceLastMove = 0f;
            reportTime = 0f;
        }
        StartCoroutine(smoothMovement(NextMovement));
    }

    private IEnumerator smoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Moving = true;
            carRigidbody.MovePosition(Vector3.MoveTowards(carRigidbody.position, end,
                inverseMoveTime * Time.deltaTime));
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        Moving = false;
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
        transform.parent.gameObject.GetComponent<CarManager>().removeCar(gameObject);
        foreach (var watcher in GameObject.FindGameObjectsWithTag("Metrics"))
            watcher.SendMessage("addJourneyTime", lifetime);
        FindObjectOfType<DispatcherAgent>().SendMessage("finishCar");

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
            case float d when d > 225 && d < 315: setDriveDirection(Direction.EAST); break;
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

    private void immediateTurnOff()
    {
        switch (driveDirection) {
            case Direction.EAST: turnDirection = Direction.NORTH; break;
            case Direction.NORTH: turnDirection = Direction.WEST; break;
            case Direction.SOUTH: turnDirection = Direction.EAST; break;
            case Direction.WEST: turnDirection = Direction.SOUTH; break;
        }

        //moveCar();
        navigateJunction(determineCorner(Physics2D.OverlapPoint(transform.position,
            traversibleLayer).transform.rotation.eulerAngles.z));
    }
}
