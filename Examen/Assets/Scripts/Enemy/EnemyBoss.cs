using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
//
// Created/Written by: Amy van Oosten
//
//

public class EnemyBoss : Enemy
{
    [Header("Enemy Boss Settings")]
    public float summonInterval = 5f; 
    public float shootingRange = 10f;
    public int bulletsPerShot = 4;
    public Transform projectileSpawn;
    public GameObject[] minion;
    public GameObject projectilePrefab;
    public LayerMask layerMask;

    private bool isShooting = false;
    private bool isSummoningMinions = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange && !isSummoningMinions)
        {
            StartCoroutine(SummoningMinions());
        }
     
        if (distanceToPlayer <= shootingRange && !isShooting)
        {
            StartCoroutine(Shooting());
        }     
        else if (distanceToPlayer > shootingRange && isShooting)
        {
            StopShooting(); 
        }
    }

    private IEnumerator SummoningMinions()
    {
        isSummoningMinions = true; 
        SummonMinions(); 
        yield return new WaitForSeconds(summonInterval);
        isSummoningMinions = false; 
    }

    private IEnumerator Shooting()
    {
        isShooting = true; 
        while (isShooting)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                ShootProjectile(projectileSpawn);
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void StopShooting()
    {
        isShooting = false; 
        isSummoningMinions = false; 
        StopCoroutine(Shooting()); 
        enemyAnimator.SetBool("ThompsonShoot", false); 
    }

    void ShootProjectile(Transform spawnPoint)
    {
        enemyAnimator.SetBool("ThompsonShoot", true); 

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity); // Instantiate projectile
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>(); // Get projectile's Rigidbody component
        Vector3 directionToPlayer = (player.position - spawnPoint.position).normalized; // Calculate direction to player
        float projectileSpeed = 20f; 
        projectileRb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse); // Add force to projectile
    }

    public void SummonMinions()
    {
        int minimum = 1;
        int maximum = 4;
        int numberOfMinionsToSpawn = Random.Range(minimum, maximum); // Randomly determine number of minions to spawn

        for (int i = 0; i < numberOfMinionsToSpawn; i++)
        {
            GameObject selectedMinionPrefab = minion[Random.Range(0, minion.Length)]; // Select a random minion prefab

            float unitSphereFactor = 4f;
            Vector3 randomOffset = Random.onUnitSphere * unitSphereFactor; // Random vector3 offset from boss position
            Vector3 minionPosition = transform.position + randomOffset; // Calculate minion position

            RaycastHit hit;
            if (Physics.Raycast(minionPosition + Vector3.up, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                minionPosition = hit.point; // Set minion position to hit point

                // Ensure minion is not too close to boss
                if (Vector3.Distance(minionPosition, transform.position) < 2f)
                {
                    continue;
                }
                Instantiate(selectedMinionPrefab, minionPosition, Quaternion.identity);
            }
        }
    }

    // Method to handle boss death
    public override void Die()
    {
        base.Die(); 
        SceneManager.LoadScene(3); 
    }
}