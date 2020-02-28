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

    public float moveTime;
    public LayerMask blockingLayer;

    private Rigidbody2D carRigidbody;
    private BoxCollider2D boxCollider;
    private float inverseMoveTime;
    public Direction direction;

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
        moveCar();
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
    }

    private IEnumerator smoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 nextPosition = Vector3.MoveTowards(carRigidbody.position, end, inverseMoveTime * Time.deltaTime);
            carRigidbody.MovePosition(nextPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    private Direction pickAnotherDirection()
    {
        Direction anotherDirection;

        do {
            anotherDirection = (Direction)Random.Range(0, 3);
        } while (anotherDirection == direction);

        return anotherDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Road") {
            float roadDirection = collision.transform.rotation.eulerAngles.z;
            Debug.Log(roadDirection.ToString());
            if (roadDirection > -45 && roadDirection < 45) direction = Direction.NORTH;
            else if (roadDirection > 45 && roadDirection < 135) direction = Direction.WEST;
            else if (roadDirection > 135 && roadDirection < 225) direction = Direction.SOUTH;
            else direction = Direction.EAST;
        }
    }
}
