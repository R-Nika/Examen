using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectile;
    public Transform projectileSpawn;
    public int projectileForce = 40;
    public bool shoot = false;
    public TMP_Text ammocountText;

    [Header("Ammo Settings")]
    public bool reload = false;
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
            if (!reload)
            {
                StartCoroutine(Reload());
            }
        }

        if (gameObject.CompareTag("Thompson")) // Check if the GameObject has the "Thompson" tag
        {
           
            if (Input.GetMouseButton(0) && ammoCount > 0 && !shoot) // Changed to GetMouseButton for continuous shooting
            {
                StartCoroutine(WaitForShoot());
            }
        }
        else 
        {
            if (Input.GetMouseButtonDown(0) && ammoCount > 0 && !shoot)
            {
                StartCoroutine(WaitForShoot());
            }
        }
    }

    public IEnumerator WaitForShoot()
    {
        shoot = true;
        Debug.Log(shoot);
        Shoot();
        shoot_audio.Play();
        yield return new WaitForSeconds(0.1f);
        shoot = false;
    }

    public IEnumerator Reload()
    {
        reload = true;
        reload_audio.Play();
        ammoCount = maxAmmo;
        yield return new WaitForSeconds(0.1f);
        reload = false;
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