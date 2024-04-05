using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 3.0f;
    public float distanceToMove = 5.0f;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition - transform.forward * distanceToMove;
    }

    void Update()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
