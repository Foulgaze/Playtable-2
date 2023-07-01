using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    int speed = 50;
    public float horiFloor = 2;
    public int scrollSpeed = 150;
    void move()
    {
        Vector3 movement = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += movement * speed * Time.deltaTime;    
    }

    void scroll()
    {
        Vector3 newPosition = transform.position + transform.forward * Input.mouseScrollDelta.y;
        if(newPosition.y > horiFloor)
        {
            transform.position += transform.forward * Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        move();
        scroll();
    }
}
