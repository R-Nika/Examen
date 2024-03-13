using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("First Person Camera Settings")]
    public bool mouseLook = true;
    public GameObject pauseMenu;

    private float mouseSensitivity = 60f;
    private float rotationSpeed = 5f;
    private float verticalRotationLimit = 80f;
    private float verticalRotation = 0f;
    private bool pauseActive = false;
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

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseActive)
        {
            pauseActive = true;
            mouseLook = false;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
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

    public void UnPause()
    {
        if ( pauseActive)
        {
            pauseActive = false;
            mouseLook = true;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
}

