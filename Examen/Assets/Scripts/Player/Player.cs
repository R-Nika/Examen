using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//
//
// Created/Written by: Amy van Oosten
//
//

public class Player : MonoBehaviour
{
    [Header("Weapon & Currency Settings")]
    public int currency;
    public GameObject weaponThompson;
    public GameObject weaponRevolver;
    public GameObject crowbar;
    private GameObject currentWeapon;

    [Header("UI Settings")]
    public TMP_Text healthText;
    public TMP_Text currencyText;

    public GameObject ammoThompsonText;
    public GameObject ammoRevolverText;

    public ArrestUIManager arrestManager;

    [Header("Player Settings")]
    private int health = 100;
    private int maxHealth = 100;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public Camera mainCamera;
    private int moveSpeed = 5;
    private int walkSpeed = 5;
            
    private int runSpeed = 10;
    private bool isCrouching = false;
    private int crouchSpeed = 2;
    private float jumpForce = 4f; 
    private bool canJump = true; 
    private bool isRunning = false;

    private int jumpcount = 0; 
    private int maxJumpcount = 2;

    private bool isWalking = false;
    private float movementThreshold = 0.01f;

    private bool isGrounded;

    [Header("Animation Settings")]
    private Animator playerAnimator;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization of variables
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        ammoThompsonText.SetActive(false);
        ammoRevolverText.SetActive(false);
        currentWeapon = weaponThompson;
        weaponThompson.SetActive(false);
        weaponRevolver.SetActive(false);
        crowbar.SetActive(false);
        playerAnimator.SetBool("Idle", true);
        playerAnimator.SetBool("Walking", false);

        health = maxHealth;
    }

    private void FixedUpdate()
    {
        Move();
        Run();
        Crouch();
    }

    // Update is called once per frame
    void Update()
    {
        // Update UI elements
        healthText.text = health.ToString();
        currencyText.text = currency.ToString();

        // Select weapons based on input
        SelectItem();

        // Manage animations and interactions
        UpdateAnimationState();
        if (Input.GetKey(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(PlayJumpAnimation());
        }
        if (health <= 0)
        {
            Die();
        }
    }

    //Handles Jump Animation
    IEnumerator PlayJumpAnimation()
    {
        playerAnimator.SetBool("Jumping", true);

        yield return new WaitForSeconds(0.3f);

        playerAnimator.SetBool("Jumping", false);
    }

    //Select current weapon/item 
    public void SelectItem()
    {
        if (Input.GetButtonDown("Select1"))
        {
            currentWeapon.SetActive(false);
           
            ammoThompsonText.SetActive(true);
            ammoRevolverText.SetActive(false);

            //Handle weapon animations
            playerAnimator.SetBool("GunHolding", false);
            playerAnimator.SetBool("ThompsonHolding", true);

            currentWeapon = weaponThompson;

            currentWeapon.SetActive(true);
        }
        else if (Input.GetButtonDown("Select2"))
        {
            currentWeapon.SetActive(false);

            ammoThompsonText.SetActive(false);
            ammoRevolverText.SetActive(true);
            
            //Handle weapon animations
            playerAnimator.SetBool("GunHolding", true);
            playerAnimator.SetBool("ThompsonHolding", false);

            currentWeapon = weaponRevolver;

            currentWeapon.SetActive(true);
        }
        else if (Input.GetButtonDown("Select3"))
        {
            currentWeapon.SetActive(false);

            ammoThompsonText.SetActive(false);
            ammoRevolverText.SetActive(false);

            //Handle weapon animations
            playerAnimator.SetBool("GunHolding", false);
            playerAnimator.SetBool("ThompsonHolding", false);

            currentWeapon = crowbar;

            currentWeapon.SetActive(true);
        }
    }

    #region Health 

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(health - amount, 0); //This so that the health will never be lower then 0 when damage is done
    }

    private void Die()
    {
        SceneManager.LoadScene(2);
    }
    #endregion

    #region Movement

    //Method for Movement of the Player
    private void Move()
    {
        // Get input for horizontal and vertical movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Vector representing the movement direction
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        isWalking = movement.magnitude > movementThreshold;
        if (isWalking)
        {
            // Normalize and scale the movement vector by moveSpeed and deltaTime
            movement = movement.normalized * moveSpeed * Time.deltaTime;

            // Calculate the new rotation based on the player's current rotation
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

            // Move the player's rigidbody to the new position
            rb.MovePosition(rb.position + newRotation * movement);
        }

        if (Input.GetButtonDown("Jump") && canJump && jumpcount <= maxJumpcount)
        {
            Debug.Log("Jump");
            jumpcount++;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Check if the player is grounded
        if (isGrounded)
        {
            Debug.Log("Grounded");
            jumpcount = 0;
            canJump = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
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
        if (Input.GetButton("Left Control") && !isRunning)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                moveSpeed = crouchSpeed;
                Vector3 newCameraPosition = mainCamera.transform.localPosition;
                newCameraPosition.y -= 0.2f; // Adjust the camera position only on the y-axis
                mainCamera.transform.localPosition = newCameraPosition;
            }
        }
        else if (isCrouching)
        {
            isCrouching = false;
            moveSpeed = isRunning ? runSpeed : walkSpeed;
            Vector3 originalCameraPosition = mainCamera.transform.localPosition;
            originalCameraPosition.y += 0.2f; // Adjust the camera position back to its original position
            mainCamera.transform.localPosition = originalCameraPosition;
        }
    }

    #endregion

    #region Animations

    public void UpdateAnimationState()
    {
        playerAnimator.SetBool("Walking", isWalking);
        playerAnimator.SetBool("Idle", !isWalking);

        playerAnimator.SetBool("Running", isRunning);

        playerAnimator.SetBool("Crouching", isCrouching);
        playerAnimator.SetBool("CrouchWalk", isCrouching && isWalking);

        UpdateWeaponAnimation(weaponThompson, "ThompsonReload", "ThompsonShoot");

        UpdateWeaponAnimation(weaponRevolver, "GunReload", "GunShoot");
    }

    private void UpdateWeaponAnimation(GameObject weapon, string reloadParam, string shootParam)
    {
        Weapon weaponComponent = weapon.GetComponent<Weapon>();

        if (weaponComponent != null)
        {
            playerAnimator.SetBool(reloadParam, weaponComponent.isReloading);
            playerAnimator.SetBool(shootParam, weaponComponent.isShooting);

        }
    }

    #endregion

    #region Interactions

    public void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange); 
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("ItemHealth")) // If ItemHealth is in range, Consume Item
                {
                    Item item = collider.GetComponent<Item>();
                    if (item != null)
                    {
                        ConsumeItem(item); 
                    }
                }

                Enemy enemy = collider.GetComponent<Enemy>(); // If Enemy is in range, Arrest Enemy

                if (enemy != null)
                {
                    arrestManager.ArrestedEnemy();
                    enemy.Arrest();
                    currency += 10;

                    break;
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
            if (health < maxHealth)  
            {
                int healthToAdd = item.healthModifyer;
                health = Mathf.Min(health + healthToAdd, maxHealth); //This so that the added health by the Item won't exceed maximum health
                item.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Health is already at the maximum. Cannot consume health item.");
            }
        }
    }

    #endregion

    
}
