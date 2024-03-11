using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Enemy
{
    [Header("Enemy Gun Settings")]
    public float shootingRange = 10f;
    public Transform projectileSpawn;
    public GameObject projectilePrefab;

    private bool isShooting = false;

    public override void Update()
    {
        base.Update();

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

    public override IEnumerator AttackRoutine()
    {
        isShooting = true;
        while (isShooting)
        {
            yield return new WaitForSeconds(1f);
            ShootProjectile();
            
        }
    }

    void ShootProjectile()
    {
        enemyAnimator.SetBool("RevolverShoot", true);

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - projectileSpawn.position).normalized;
        float projectileSpeed = 20f;
        projectileRb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);
    }
}