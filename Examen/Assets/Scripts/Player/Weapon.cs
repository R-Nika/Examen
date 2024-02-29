using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
   
    public GameObject projectile;
    public Transform projectileSpawn;
    public int projectileForce = 40;

    [SerializeField] private int ammoCount = 10;
    [SerializeField] private int maxAmmo = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetMouseButtonDown(0) && ammoCount > 0)
        {
            Shoot();
        }
    }

    public void Reload()
    {
        // You might want to check if the player can reload (e.g., not in the middle of shooting)
        // For simplicity, let's assume the player can always reload

        // Reset ammo count to the maximum
        ammoCount = maxAmmo;
    }

    public void Shoot()
    {
        // Instantiate the projectile at the specified spawn point
        GameObject projectileInst = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);

        // Add force to the projectile in the forward direction
        Rigidbody projectileRb = projectileInst.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(projectileSpawn.forward * projectileForce, ForceMode.Impulse);

            // Decrease ammo count
            ammoCount--;
        }
        else
        {
            Debug.LogError("Projectile is missing Rigidbody component!");
        }
    }

}
