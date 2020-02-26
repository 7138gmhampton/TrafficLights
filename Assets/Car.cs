using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveTime;

    private Rigidbody2D carRigidbody;
    private float inverseMoveTime;

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
}
