using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public Transform projectileSpawn;
    public int projectileForce = 40;

    [Header("Ammo Settings")]
    [SerializeField] private int ammoCount = 10;
    [SerializeField] private int maxAmmo = 10;

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
        ammoCount = maxAmmo;
    }

    public void Shoot()
    {
        GameObject projectileInst = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);

        Rigidbody projectileRb = projectileInst.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(projectileSpawn.forward * projectileForce, ForceMode.Impulse);
            ammoCount--;
        }
        else
        {
            Debug.LogError("Projectile is missing Rigidbody component!");
        }
    }

}
