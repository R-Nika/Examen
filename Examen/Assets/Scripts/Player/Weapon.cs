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


    // Update is called once per frame
    void Update()
    {
        ammocountText.text = ammoCount.ToString() + "/" + maxAmmo.ToString();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!reload)
            {
                StartCoroutine(Reload());

            }
        }
    
        if (Input.GetMouseButtonDown(0) && ammoCount > 0 && !shoot)
        {
            StartCoroutine(WaitForShoot());
        }
    }

    public IEnumerator WaitForShoot()
    {
        shoot = true;
        Debug.Log(shoot);
        Shoot();

        shoot_audio.Play();

        yield return new WaitForSeconds(0.2f);
        shoot = false;
    }

    public IEnumerator Reload()
    {

        reload = true;

        reload_audio.Play();
        ammoCount = maxAmmo;
        yield return new WaitForSeconds(0.2f);
        reload = false;
        Debug.Log(reload);

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
