using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    private enum Corner
    {
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        ERROR
    }

    public float moveTime;
    public LayerMask blockingLayer;

    private Rigidbody2D carRigidbody;
    private BoxCollider2D boxCollider;
    private float inverseMoveTime;
    public Direction direction;
    private Direction turnDirection;
    private bool moving;
    private bool turning;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        inverseMoveTime = 1f / moveTime;
        //direction = Direction.EAST;
    }

    void Update()
    {
        //var nextTile = transform.position;
        //nextTile.y += 1.0f;

        //StartCoroutine(smoothMovement(nextTile));
        if (!moving) moveCar();
    }

    private void moveCar()
    {
        Vector2 end;
        switch (direction) {
            case Direction.NORTH:
                //end = (Vector2)transform.position + new Vector2(0.0f, 1.0f);
                //StartCoroutine(smoothMovement(end));
                AttemptMove(0, 1);
                break;
            case Direction.EAST:
                //end = (Vector2)transform.position + new Vector2(1.0f, 0.0f);
                //StartCoroutine(smoothMovement(end));
                AttemptMove(1, 0);
                break;
            case Direction.SOUTH:
                //end = (Vector2)transform.position + new Vector2(0.0f, -1.0f);
                //StartCoroutine(smoothMovement(end));
                AttemptMove(0, -1);
                break;
            case Direction.WEST:
                //end = (Vector2)transform.position + new Vector2(-1.0f, 0.0f);
                //StartCoroutine(smoothMovement(end));
                AttemptMove(-1, 0);
                break;
        }
    }

    private void AttemptMove(int deltaX, int deltaY)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(deltaX, deltaY);

        boxCollider.enabled = false;
        var blocked = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (blocked.transform == null)
            StartCoroutine(smoothMovement(end));
            //directMove(end);
    }

    private IEnumerator smoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            moving = true;
            Vector3 nextPosition = Vector3.MoveTowards(carRigidbody.position, end, inverseMoveTime * Time.deltaTime);
            carRigidbody.MovePosition(nextPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        moving = false;
    }

    private void directMove(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 nextPosition = Vector3.MoveTowards(carRigidbody.position, end, inverseMoveTime * Time.deltaTime);
            carRigidbody.MovePosition(nextPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        }
    }

    private Direction pickAnotherDirection(Direction previousDirection)
    {
        Direction anotherDirection;

        do {
            anotherDirection = (Direction)Random.Range(0, 4);
        } while (anotherDirection == getOpposite(previousDirection));
        //Debug.Log(anotherDirection.ToString());
        return anotherDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Road") {
            //turnDirection = null;
            turning = false;
            float roadDirection = collision.transform.rotation.eulerAngles.z;
            //Debug.Log(roadDirection.ToString());
            if (roadDirection > -45 && roadDirection < 45) direction = Direction.NORTH;
            else if (roadDirection > 45 && roadDirection < 135) direction = Direction.WEST;
            else if (roadDirection > 135 && roadDirection < 225) direction = Direction.SOUTH;
            else direction = Direction.EAST;
            //moveCar();
        }
        else if (collision.tag == "Junction") {
            if (!turning) {
                turnDirection = pickAnotherDirection(direction);
                Debug.Log("Turning " + turnDirection.ToString());
                turning = true;
            }

            //float junctionCornerRotation = collision.transform.rotation.eulerAngles.z;
            //Debug.Log("Angle for Corner " + junctionCornerRotation.ToString());
            //Debug.Log("Corner Rot: " + collision.transform.eulerAngles.z.ToString());

            //if (junctionCornerRotation > 225 && junctionCornerRotation < 315) {
            //    Debug.Log("Top Left");
            //    if (turnDirection == Direction.NORTH) direction = Direction.NORTH;
            //    else direction = Direction.EAST;
            //}
            //if (junctionCornerRotation > 135 && junctionCornerRotation < 225) {
            //    if (turnDirection == Direction.EAST) direction = Direction.EAST;
            //    else direction = Direction.SOUTH;
            //}
            //if (junctionCornerRotation > 45 && junctionCornerRotation < 135) {
            //    if (turnDirection == Direction.SOUTH) direction = Direction.SOUTH;
            //    else direction = Direction.WEST;
            //}
            //else {
            //    if (turnDirection == Direction.WEST) direction = Direction.WEST;
            //    else direction = Direction.NORTH;
            //}
            //Debug.Log("Going" + direction.ToString());

            //moveCar();
            var corner = determineCorner(collision.transform.eulerAngles.z);
            Debug.Log(corner.ToString());
            Debug.Log(turning.ToString());

            switch (corner) {
                case Corner.TOP_LEFT:
                    if (turnDirection == Direction.NORTH) direction = Direction.NORTH;
                    else direction = Direction.EAST;
                    break;
                case Corner.TOP_RIGHT:
                    if (turnDirection == Direction.EAST) direction = Direction.EAST;
                    else direction = Direction.SOUTH;
                    break;
                case Corner.BOTTOM_RIGHT:
                    if (turnDirection == Direction.SOUTH) direction = Direction.SOUTH;
                    else direction = Direction.WEST;
                    break;
                case Corner.BOTTOM_LEFT:
                    if (turnDirection == Direction.WEST) direction = Direction.WEST;
                    else direction = Direction.NORTH;
                    break;
            }

            Debug.Log("Going " + direction.ToString());
        }
    }

    private Corner determineCorner(float cornerRotation)
    {
        if (cornerRotation > -45 && cornerRotation < 45) return Corner.BOTTOM_LEFT;
        else if (cornerRotation > 45 && cornerRotation < 135) return Corner.BOTTOM_RIGHT;
        else if (cornerRotation > 135 && cornerRotation < 225) return Corner.TOP_RIGHT;
        else if (cornerRotation > 225 && cornerRotation < 315) return Corner.TOP_LEFT;

        return Corner.ERROR;
    }

    private Direction getOpposite(Direction directionForward)
    {
        Dictionary<Direction, Direction> opposites = new Dictionary<Direction, Direction>() 
        {
            { Direction.NORTH, Direction.SOUTH },
            { Direction.EAST, Direction.WEST },
            { Direction.SOUTH, Direction.NORTH },
            { Direction.WEST, Direction.EAST }
        };

        return opposites[directionForward];
    }
}
