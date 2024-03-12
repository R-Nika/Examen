using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public TMP_Text arrestText;
    public TMP_Text currencyText;

    public GameObject ammoThompsonText;
    public GameObject ammoRevolverText;

    public GameObject gameOverScreen;

    [Header("Player Settings")]
    private int health = 100;
    private Rigidbody rb;


    [Header("Movement Settings")]
    public Camera mainCamera;
    public Vector3 cameraPosNormal = new Vector3(0,1.8f,1);
    public Vector3 cameraPosCrouch = new Vector3(0,1.6f,1);
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int walkSpeed = 5;
            
    [SerializeField] private int runSpeed = 10;
    [SerializeField] private bool isCrouching = false;
    [SerializeField] private int crouchSpeed = 2;
    [SerializeField] private float jumpForce = 4f; 
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool canJump = true; 
    [SerializeField] private bool isRunning = false;

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
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        ammoThompsonText.SetActive( false);
        ammoRevolverText.SetActive(false);

        currentWeapon = weaponThompson;

        weaponThompson.SetActive(false);
        weaponRevolver.SetActive(false);
        crowbar.SetActive(false);

        arrestText.enabled = false;

        gameOverScreen.SetActive(false);

        playerAnimator.SetBool("Idle", true);
        playerAnimator.SetBool("Walking", false);
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        currencyText.text = currency.ToString();

        Move();
        Run();
        Crouch();
        SelectItem();

        AnimationManager();

        if (Input.GetButtonDown("Interaction"))
        {
            ArrestClosestEnemy();
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

    IEnumerator PlayJumpAnimation()
    {
        isJumping = true;
        playerAnimator.SetBool("Jumping", true);

        yield return new WaitForSeconds(0.3f);

        playerAnimator.SetBool("Jumping", false);
        isJumping = false;
    }


    public void SelectItem()
    {
        if (Input.GetButtonDown("Select1"))
        {
            currentWeapon.SetActive(false);
           
            ammoThompsonText.SetActive(true);
            ammoRevolverText.SetActive(false);
            
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

            playerAnimator.SetBool("GunHolding", false);
            playerAnimator.SetBool("ThompsonHolding", false);

            currentWeapon = crowbar;

            currentWeapon.SetActive(true);
        }
    }

    #region Health 

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        SceneManager.LoadScene(2);
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

    private bool IsGrounded()
    {
        return isGrounded;
    }

    private void Run()
    {
        if (Input.GetButtonDown("Left Shift") && !isCrouching)
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

    public void AnimationManager()
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
            playerAnimator.SetBool(reloadParam, weaponComponent.reload);
            playerAnimator.SetBool(shootParam, weaponComponent.shoot);

            if (weaponComponent.reload)
            {
                Debug.Log($"Anim {weapon.name} Reload");
            }
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
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("ItemHealth"))
                {
                    Item item = collider.GetComponent<Item>();
                    if (item != null)
                    {
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
            if (health < 100)  
            {
                int healthToAdd = Mathf.Min(100 - health, item.healthModifyer);
                health += healthToAdd;

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
