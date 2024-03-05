using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int damage;
    public float shootingRange = 10f;
    public float stoppingDistance = 5f;
    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    private Transform player;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            Attack();
        }
    }

    public virtual void Attack()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Vector3 newPosition = player.position - directionToPlayer.normalized * stoppingDistance;
            navMeshAgent.SetDestination(newPosition);
        }
        else
        {
            navMeshAgent.ResetPath();
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);

            for (int i = 0; i < 6; i++)
            {
                ShootProjectile();
                yield return new WaitForSeconds(0.1f);
            }
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
