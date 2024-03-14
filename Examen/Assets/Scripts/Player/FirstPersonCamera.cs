using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// Created/Written by: Amy van Oosten
//
//

public class FirstPersonCamera : MonoBehaviour
{
    [Header("First Person Camera Settings")]
    public bool mouseLook = true;
    public GameObject pauseMenu;

    private float mouseSensitivity = 50f;
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

        // Check for pause key input
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseActive)
        {          
            pauseActive = true;
            mouseLook = false; 
            pauseMenu.SetActive(true); 
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 
            Time.timeScale = 0; // Set time scale to 0 to pause the game
        }
    }

    // Method to handle mouse look functionality
    private void HandleMouseLook()
    {
        // Get mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * rotationSpeed * Time.deltaTime;

        // Adjust vertical rotation based on mouseY input and clamp within limits
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Rotate camera vertically based on verticalRotation
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Rotate player horizontally based on mouseX input
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    public void UnPause()
    {
        if (pauseActive)
        {          
            pauseActive = false; 
            mouseLook = true; 
            pauseMenu.SetActive(false); 
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false; 
            Time.timeScale = 1; // Set time scale back to normal (1)
        }
    }
}