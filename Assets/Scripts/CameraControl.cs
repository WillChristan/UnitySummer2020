using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotSpeed;
    public Transform target, player;
    float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (rotSpeed <= 0.0f) rotSpeed = 1.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * rotSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotSpeed;
        mouseY = Mathf.Clamp(mouseY, -35.0f, 60.0f);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(mouseY, mouseX, 0.0f);
        //player.rotation = Quaternion.Euler(0.0f, mouseX, 0.0f);
    }
}
