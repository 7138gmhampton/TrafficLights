﻿using System.Collections;
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
    private Direction driveDirection;
    private Direction turnDirection;
    private bool moving;
    private bool turning;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        inverseMoveTime = 1f / moveTime;
    }

    void Update()
    {
        if (!moving) moveCar();
    }

    private void moveCar()
    {
        switch (driveDirection) {
            case Direction.NORTH:
                AttemptMove(0, 1);
                break;
            case Direction.EAST:
                AttemptMove(1, 0);
                break;
            case Direction.SOUTH:
                AttemptMove(0, -1);
                break;
            case Direction.WEST:
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
        return anotherDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Road") {
            turning = false;
            float roadDirection = collision.transform.rotation.eulerAngles.z;
            
            if (roadDirection > -45 && roadDirection < 45) driveDirection = Direction.NORTH;
            else if (roadDirection > 45 && roadDirection < 135) driveDirection = Direction.WEST;
            else if (roadDirection > 135 && roadDirection < 225) driveDirection = Direction.SOUTH;
            else driveDirection = Direction.EAST;
        }
        else if (collision.tag == "Junction") {
            if (!turning) {
                turnDirection = pickAnotherDirection(driveDirection);
                Debug.Log("Turning " + turnDirection.ToString());
                turning = true;
            }

            var corner = determineCorner(collision.transform.eulerAngles.z);
            Debug.Log(corner.ToString());
            Debug.Log(turning.ToString());

            switch (corner) {
                case Corner.TOP_LEFT:
                    if (turnDirection == Direction.NORTH) driveDirection = Direction.NORTH;
                    else driveDirection = Direction.EAST;
                    break;
                case Corner.TOP_RIGHT:
                    if (turnDirection == Direction.EAST) driveDirection = Direction.EAST;
                    else driveDirection = Direction.SOUTH;
                    break;
                case Corner.BOTTOM_RIGHT:
                    if (turnDirection == Direction.SOUTH) driveDirection = Direction.SOUTH;
                    else driveDirection = Direction.WEST;
                    break;
                case Corner.BOTTOM_LEFT:
                    if (turnDirection == Direction.WEST) driveDirection = Direction.WEST;
                    else driveDirection = Direction.NORTH;
                    break;
            }

            Debug.Log("Going " + driveDirection.ToString());
        }
        else if (collision.tag == "Despawner")
            Destroy(gameObject);
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
