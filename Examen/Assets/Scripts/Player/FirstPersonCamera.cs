using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
 
    private float mouseSensitivity = 2f;
    private float rotationSpeed = 5f;
    private float verticalRotationLimit = 80f;
    private float verticalRotation = 0f;

    private Transform playerTransform; // NEW VARIABLE


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerTransform = transform.parent; // Assuming camera is a child of the player
    }

    void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * rotationSpeed;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}

