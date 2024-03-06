using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int damage;
    public float stoppingDistance = 5f;
    public float sightRange = 10f; // New sight range variable
    public Transform player;
    private NavMeshAgent navMeshAgent;
    public Slider healthSlider;


    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        MoveAndAttack();
        
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void MoveAndAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange)
        {
            if (distanceToPlayer <= stoppingDistance)
            {
               
                navMeshAgent.ResetPath();
              
            }
            else
            {
               
                Vector3 directionToPlayer = player.position - transform.position;
                Vector3 newPosition = player.position - directionToPlayer.normalized * stoppingDistance;
                navMeshAgent.SetDestination(newPosition);
            }
        }
        else
        {
            
            navMeshAgent.ResetPath();
           
        }
    }

    public virtual IEnumerator AttackRoutine()
    {
            yield return new WaitForSeconds(2f);
      
    }
}