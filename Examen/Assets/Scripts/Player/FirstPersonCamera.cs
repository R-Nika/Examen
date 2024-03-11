using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
 
    private float mouseSensitivity = 60f;
    private float rotationSpeed = 5f;
    private float verticalRotationLimit = 80f;
    private float verticalRotation = 0f;

    private Transform playerTransform; // NEW VARIABLE


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerTransform = transform.parent; // Assuming camera is a child of the player
    }

    void FixedUpdate()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * rotationSpeed * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}

