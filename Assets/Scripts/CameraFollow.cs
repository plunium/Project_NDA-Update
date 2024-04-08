using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform[] Pos;
    public float CameraSpeed;

    private float _timer = 0.0f;

    public int IdPos;

    Vector3 offset;
    private void Start()
    {
        // offset = transform.position - player.position;
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, Pos[IdPos].position, CameraSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Pos[IdPos].rotation, CameraSpeed);
    }

    public void setRun()
    {
        IdPos = 0;
    }

    public void setSlide()
    {
        IdPos = 1;
    }

    public void setJump()
    {
        IdPos = 2;
    }

    public void setTurnLeft()
    {
        IdPos = 3;
    }

    public void setTurnRight()
    {
        IdPos = 4;
    }
}
