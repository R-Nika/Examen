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

    private Transform player;
    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        // Implement shooting logic here
        // For example, instantiate bullets, play shooting animation, etc.
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
        // Add death logic here (e.g., play death animation, spawn particles, etc.)
        Destroy(gameObject);
    }

    public void Move()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            // Move towards the player while maintaining a certain distance
            Vector3 directionToPlayer = player.position - transform.position;
            Vector3 newPosition = player.position - directionToPlayer.normalized * stoppingDistance;
            navMeshAgent.SetDestination(newPosition);
        }
        else
        {
            // Stop moving if within the stopping distance
            navMeshAgent.ResetPath();
        }
    }

}
