using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//
//
// Created/Written by: Amy van Oosten
//
//

public class Weapon : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public Transform projectileSpawn;
    public int projectileForce = 40;
    public bool isShooting = false;
    public TMP_Text ammocountText;

    [Header("Ammo Settings")]
    public bool isReloading = false;
    [SerializeField] private int ammoCount = 10;
    [SerializeField] private int maxAmmo = 10;

    [Header("Audio Settings")]
    public AudioSource shoot_audio;
    public AudioSource reload_audio;

    void Update()
    {
        ammocountText.text = ammoCount.ToString() + "/" + maxAmmo.ToString();

        if (Input.GetButtonDown("Reload"))
        {
            if (!isReloading)
            {
                StartCoroutine(Reload());
            }
        }
        // Check if gun has enough Ammo
        bool hasRemainingAmmo = ammoCount > 0;
        if (gameObject.CompareTag("Thompson")) 
        {
           
            if (Input.GetMouseButton(0) && hasRemainingAmmo && !isShooting) // Changed for continuous shooting
            {
                StartCoroutine(WaitForShoot());
            }
        }
        else 
        {
            if (Input.GetMouseButtonDown(0) && hasRemainingAmmo && !isShooting)
            {
                StartCoroutine(WaitForShoot());
            }
        }
    }

    // Coroutine for a delay in shooting
    public IEnumerator WaitForShoot()
    {
        isShooting = true;
        Debug.Log(isShooting);
        Shoot();
        shoot_audio.Play();
        yield return new WaitForSeconds(0.1f);
        isShooting = false;
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        reload_audio.Play();
        //Reload Ammo
        ammoCount = maxAmmo;
        yield return new WaitForSeconds(0.1f);
        isReloading = false;
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