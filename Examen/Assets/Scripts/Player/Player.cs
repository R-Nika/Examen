using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject weaponA;
    public GameObject weaponB;
    public GameObject crowbar;
    [SerializeField] private int health = 100;
    public TMP_Text healthText;
    
    [SerializeField]private GameObject currentWeapon;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int runSpeed = 10;
    [SerializeField] private int crouchSpeed = 2;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isCrouching = false;

    // new variables not in classdiagram
    [SerializeField] private float jumpForce = 4f; 
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool canJump = true; 
    [SerializeField] private bool isRunning = false;
    [SerializeField] private int walkSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWeapon = weaponA;
        weaponA.SetActive(false);
        weaponB.SetActive(false);
        crowbar.SetActive(false);

    
        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        Debug.Log(health);


        Run();
        Crouch();
        Move();
        SelectItem();

    }


    public void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon.SetActive(false);
            currentWeapon = weaponA;
            currentWeapon.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon.SetActive(false);
            currentWeapon = weaponB;
            currentWeapon.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon.SetActive(false);
            currentWeapon = crowbar;
            currentWeapon.SetActive(true);
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Player took damage: " + amount);

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }


    public void Interact()
    {
    
    }
 
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        rb.MovePosition(rb.position + newRotation * movement);

        if (Input.GetButtonDown("Jump") && IsGrounded() && canJump)  
        {
            isJumping = true;  
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false; 
        }

        if (IsGrounded())
        {
            isJumping = false;
            canJump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }


    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            isRunning = true;
            moveSpeed = runSpeed;
        }
        else
        {
            isRunning = false;
            moveSpeed = walkSpeed;
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isRunning)
        {
            isCrouching = true;
            moveSpeed = crouchSpeed;

 
            Vector3 newCameraPosition = new Vector3(0f, 0.4f, 0f);
            transform.GetChild(0).localPosition = newCameraPosition;
        }
        else
        {
            isCrouching = false;
            moveSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 originalCameraPosition = new Vector3(0f, 0.8f, 0f);
            transform.GetChild(0).localPosition = originalCameraPosition;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }
}
