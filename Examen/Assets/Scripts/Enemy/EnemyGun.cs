using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Enemy
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float shootingRange = 10f;

    private bool isShooting = false;

    public override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= shootingRange && !isShooting)
        {
            StartCoroutine(AttackRoutine());
        }
        else if (distanceToPlayer > shootingRange && isShooting)
        {
            StopCoroutine(AttackRoutine());
            isShooting = false;
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
        Debug.Log("Shooting projectile");
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - projectileSpawn.position).normalized;
        float projectileSpeed = 20f;
        projectileRb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);
    }
}