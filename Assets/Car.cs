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
        WEST
    }

    public float moveTime;

    private Rigidbody2D carRigidbody;
    private float inverseMoveTime;
    private Direction direction;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    void Update()
    {
        var nextTile = transform.position;
        nextTile.y += 1.0f;

        StartCoroutine(smoothMovement(nextTile));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Road") {
            float roadDirection = collision.transform.rotation.z;
            if (roadDirection > -45 && roadDirection < 45) direction = Direction.NORTH;
            else if (roadDirection > 45 && roadDirection < 135) direction = Direction.EAST;
            else if (roadDirection > 135 && roadDirection < 225) direction = Direction.SOUTH;
            else direction = Direction.WEST;
        }
    }
}
