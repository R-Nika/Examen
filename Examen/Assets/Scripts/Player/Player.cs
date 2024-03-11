using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Weapon & Currency Settings")]
    public int currency;
    public GameObject weaponThompson;
    public GameObject weaponRevolver;
    public GameObject crowbar;
    [SerializeField] private GameObject currentWeapon;

    [Header("UI Settings")]
    public TMP_Text healthText;
    public TMP_Text arrestText;
    public TMP_Text currencyText;

    public GameObject ammoThompsonText;
    public GameObject ammoRevolverText;

    [Header("Player Settings")]
    [SerializeField] private int health = 100;
    [SerializeField] private Rigidbody rb;


    [Header("Movement Settings")]
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int walkSpeed = 5;
            
    [SerializeField] private int runSpeed = 10;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private int crouchSpeed = 2;
    [SerializeField] private float jumpForce = 4f; 
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool canJump = true; 
    [SerializeField] private bool isRunning = false;

    public Camera mainCamera;

    private int jumpcount = 0;  // Move jumpcount outside the Move method
    private int maxJumpcount = 2;


    private bool isWalking = false;
    private float movementThreshold = 0.01f;


    [Header("Animation Settings")]
    private Animator playerAnimator;


    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;

   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        ammoThompsonText.SetActive( false);
        ammoRevolverText.SetActive(false);

        currentWeapon = weaponThompson;

        weaponThompson.SetActive(false);
        weaponRevolver.SetActive(false);
        crowbar.SetActive(false);
        arrestText.enabled = false;

        playerAnimator.SetBool("Idle", true);
        playerAnimator.SetBool("Walking", false);


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

        
        Move();
        Run();
        Crouch();
        SelectItem();

        AnimationManager();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ArrestClosestEnemy();
            Interact();
        }
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(PlayJumpAnimation());
        }



    }

    IEnumerator PlayJumpAnimation()
    {
        isJumping = true;

        playerAnimator.SetBool("Jumping", true);
        Debug.Log("Jump button pressed");

        // Wait for 0.3 seconds (adjust the time as needed)
        yield return new WaitForSeconds(0.3f);

        playerAnimator.SetBool("Jumping", false);

        isJumping = false;
    }


    public void SelectItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon.SetActive(false);
            currentWeapon = weaponThompson;

            ammoThompsonText.SetActive(true);
            ammoRevolverText.SetActive(false);
            
            playerAnimator.SetBool("GunHolding", false);
            playerAnimator.SetBool("ThompsonHolding", true);

            currentWeapon.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon.SetActive(false);
            ammoThompsonText.SetActive(false);
            ammoRevolverText.SetActive(true);
          
            playerAnimator.SetBool("GunHolding", true);
            playerAnimator.SetBool("ThompsonHolding", false);

            currentWeapon = weaponRevolver;

            currentWeapon.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWeapon.SetActive(false);

            playerAnimator.SetBool("GunHolding", false);
            playerAnimator.SetBool("ThompsonHolding", false);

            currentWeapon = crowbar;

            ammoThompsonText.SetActive(false);
            ammoRevolverText.SetActive(false);

            currentWeapon.SetActive(true);
        }
    }

    #region Health 

    public void TakeDamage(int amount)
    {
        Debug.Log("Player took damage: " + amount);

        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }
    #endregion

    #region Movement
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        isWalking = (new Vector3(horizontal, 0, vertical).magnitude > movementThreshold); // Check if the movement magnitude is greater than the threshold

        Debug.Log("Walking: " + isWalking);

        Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        rb.MovePosition(rb.position + newRotation * movement);

        if (Input.GetButtonDown("Jump") && canJump && jumpcount <= maxJumpcount)
        {
            Debug.Log("Jump");
            jumpcount++;
            isJumping = true;
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
      
        if (IsGrounded())
        {
            Debug.Log("Grounded");
            // jumpcount should not be reset here
            jumpcount = 0;
           // isJumping = false;
            
            canJump = true;
        }
    }
  
    private bool isGrounded;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object having the "floor" tag
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset the grounded state when leaving the collision with the "floor" tag
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private bool IsGrounded()
    {
        return isGrounded;
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
            if (!isCrouching)
            {
                isCrouching = true;
                moveSpeed = crouchSpeed;

                Vector3 newCameraPosition = new Vector3(0f, 1.8f, 0.1f); // Camera height when crouching // hardcoded
                mainCamera.transform.position = newCameraPosition;
            }
        }
        else if (isCrouching) // Check if currently crouching
        {
            isCrouching = false;
            moveSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 originalCameraPosition = new Vector3(0f, 1.6f, 0.1f); // NOT HARDCODED
            mainCamera.transform.position = originalCameraPosition;
        }

    }

    #endregion


    public void AnimationManager()
    {
        // Walking and Idle States
        if (isWalking)
        {
            playerAnimator.SetBool("Walking", true);
            playerAnimator.SetBool("Idle", false);
        }
        else
        {
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("Idle", true);
        }

        // Running State
        if (isRunning)
        {
            playerAnimator.SetBool("Running", true);
        }
        else
        {
            playerAnimator.SetBool("Running", false);
        }

        
        // Crouch States
        if (isCrouching && isWalking)
        {
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("CrouchWalk", true);
            playerAnimator.SetBool("Crouching", false);
            playerAnimator.SetBool("Idle", false);
        }
        else if (!isCrouching && isWalking)
        {
            playerAnimator.SetBool("Walking", true);
            playerAnimator.SetBool("CrouchWalk", false);
            playerAnimator.SetBool("Crouching", false);
            playerAnimator.SetBool("Idle", false);
        }
        else if (isCrouching)
        {
            playerAnimator.SetBool("Walking", false);
            playerAnimator.SetBool("CrouchWalk", false);
            playerAnimator.SetBool("Crouching", true);
            playerAnimator.SetBool("Idle", false);
        }
        else
        {
            playerAnimator.SetBool("Crouching", false);
        }



        
        if (weaponThompson.GetComponent<Weapon>().reload)
        {
            playerAnimator.SetBool("ThompsonReload", true);
            Debug.Log("Anim Thompson Reload");
        }
        else
        {
            playerAnimator.SetBool("ThompsonReload", false);
        }

        if (weaponThompson.GetComponent<Weapon>().shoot)
        {
            playerAnimator.SetBool("ThompsonShoot", true);
        }
        else
        {
            playerAnimator.SetBool("ThompsonShoot", false);

        }



        if (weaponRevolver.GetComponent<Weapon>().reload)
        {
            playerAnimator.SetBool("GunReload", true);
            Debug.Log("Anim Revolver Reload");
        }
        else
        {
            playerAnimator.SetBool("GunReload", false);
        }

        if (weaponRevolver.GetComponent<Weapon>().shoot)
        {
            playerAnimator.SetBool("GunShoot", true);
        }
        else
        {
            playerAnimator.SetBool("GunShoot", false);

        }

    }

    #region Interactions

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

    #endregion

    
}
