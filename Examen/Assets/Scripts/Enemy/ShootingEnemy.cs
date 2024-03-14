using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// Created/Written by: Amy van Oosten
//
//

public class ShootingEnemy : Enemy
{
    [Header("Enemy Gun Settings")]
    public float shootingRange = 10f;
    public Transform projectileSpawn;
    public GameObject projectilePrefab;

    private bool isShooting = false;

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
     
        if (distanceToPlayer <= shootingRange && !isShooting)
        {
            StartCoroutine(AttackRoutine()); 
            enemyAnimator.SetBool("GunWalk", true); 
        }
        else if (distanceToPlayer > shootingRange && isShooting)
        {
            StopCoroutine(AttackRoutine()); 
            isShooting = false;
            enemyAnimator.SetBool("GunWalk", false);
        }
    }

    // Coroutine for attacking
    public override IEnumerator AttackRoutine()
    {
        isShooting = true; 
        while (isShooting)
        {
            yield return new WaitForSeconds(1f); 
            ShootProjectile();
        }
    }

    // Method to shoot projectile
    void ShootProjectile()
    {
        enemyAnimator.SetBool("RevolverShoot", true); 

        // Instantiate projectile and set its direction and speed
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - projectileSpawn.position).normalized;
        float projectileSpeed = 20f;
        projectileRb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);
    }
}