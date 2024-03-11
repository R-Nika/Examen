using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("First Person Camera Settings")]
    public bool mouseLook = true;

    private float mouseSensitivity = 60f;
    private float rotationSpeed = 5f;
    private float verticalRotationLimit = 80f;
    private float verticalRotation = 0f;
    private Transform playerTransform; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerTransform = transform.parent;
    }

    void FixedUpdate()
    {
        if (mouseLook)
        {
            HandleMouseLook();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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

