using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public override void Start()
    {
        base.Start();
        
    }

    public override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange && !isSummoningMinions)
        {
            StartCoroutine(SummonMinionsRoutine());
        }

        if (distanceToPlayer <= shootingRange && !isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
        else if (distanceToPlayer > shootingRange && isShooting)
        {
            StopShooting();
        }
    }

    private IEnumerator SummonMinionsRoutine()
    {
        isSummoningMinions = true;
        SummonMinions();
        yield return new WaitForSeconds(summonInterval);
        isSummoningMinions = false;
    }

    private IEnumerator ShootRoutine()
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
        StopCoroutine(ShootRoutine());

        enemyAnimator.SetBool("ThompsonShoot", false);
    }

    void ShootProjectile(Transform spawnPoint)
    {
        enemyAnimator.SetBool("ThompsonShoot", true);

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.position - spawnPoint.position).normalized;
        float projectileSpeed = 20f;
        projectileRb.AddForce(directionToPlayer * projectileSpeed, ForceMode.Impulse);
    }

    public void SummonMinions()
    {
        int numberOfMinionsToSpawn = Random.Range(1, 4); 

        for (int i = 0; i < numberOfMinionsToSpawn; i++)
        {
            GameObject selectedMinionPrefab = minion[Random.Range(0, minion.Length)];

            Vector3 randomOffset = Random.onUnitSphere * 4f;
            Vector3 minionPosition = transform.position + randomOffset;

            RaycastHit hit;
            if (Physics.Raycast(minionPosition + Vector3.up * 1f, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                //if (hit.collider.CompareTag("Floor"))
                //{
                    minionPosition = hit.point;

                    if (Vector3.Distance(minionPosition, transform.position) < 2f)
                    {
                        continue;
                    }
                    Instantiate(selectedMinionPrefab, minionPosition, Quaternion.identity);
                //}
            }
        }
    }

    public override void Die()
    {
        base.Die();
        
        SceneManager.LoadScene(3);
    }
}