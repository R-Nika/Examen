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
    public TMP_Text arrestText;

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
    [SerializeField] private float interactionRange = 3f;

    public int currency;
    public TMP_Text currencyText;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWeapon = weaponA;
        weaponA.SetActive(false);
        weaponB.SetActive(false);
        crowbar.SetActive(false);
        arrestText.enabled = false;


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
        currencyText.text = currency.ToString();

        Run();
        Crouch();
        Move();
        SelectItem();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ArrestClosestEnemy();
            Interact();
        }

     
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


    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        rb.MovePosition(rb.position + newRotation * movement);

        int jumpcount = 0;
        int maxJumpcount = 2;

        if (Input.GetButtonDown("Jump") && IsGrounded() && canJump && jumpcount < maxJumpcount)
        {
            jumpcount++;
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }

        if (IsGrounded())
        {
            jumpcount = 0;
            isJumping = false;
            canJump = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
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

    public void ArrestClosestEnemy()
    {
        int arrestRange = 1;

        Collider[] colliders = Physics.OverlapSphere(transform.position, arrestRange);

        foreach (var collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Arrest();
                currency += 10;
                
                StartCoroutine(ArrestText());
                break;
            }
        }
    }

    public IEnumerator ArrestText()
    {
        arrestText.enabled = true;
        yield return new WaitForSeconds(2f);
        arrestText.enabled = false;
    }

    public void Interact()
    {
      
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);

            if (colliders.Length > 0)
            {
                Debug.Log("Interact called. Colliders detected: " + colliders.Length);

                foreach (var collider in colliders)
                {
                    Debug.Log("Collider tag: " + collider.tag);

                    if (collider.CompareTag("ItemHealth"))
                    {
                        Debug.Log("ItemHealth detected!");

                        // Check if the collider has the Item script
                        Item item = collider.GetComponent<Item>();

                        if (item != null)
                        {
                            Debug.Log("Item script found!");

                            ConsumeItem(item);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Interact called. No colliders detected.");
            }
        
    }

    void ConsumeItem(Item item)
    {
        if (item.itemtype == ItemType.HealthItem)
        {
            if (health < 100)  // Check if health is less than the maximum value
            {
                int healthToAdd = Mathf.Min(100 - health, item.healthModifyer);
                health += healthToAdd;

                // Disable the item after consumption
                item.gameObject.SetActive(false);

                Debug.Log("Health increased by " + healthToAdd + ". Current health: " + health);
            }
            else
            {
                Debug.Log("Health is already at the maximum. Cannot consume health item.");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }
}
