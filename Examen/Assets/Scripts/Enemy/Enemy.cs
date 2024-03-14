using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

//
//
// Created/Written by: Amy van Oosten
//
//

public class Enemy : MonoBehaviour
{
    [Header("Enemy Base Settings")]
    public int health = 100;
    public int damage;
    public float stoppingDistance = 5f;
    public float sightRange = 10f;
    public bool isWalking = false;
    public Transform player;
    public Slider healthSlider;
    public Animator enemyAnimator;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    public virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        enemyAnimator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {            
            Debug.LogError("Player GameObject not found with tag 'Player'. Make sure the player has the correct tag.");
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Make the enemy look at the player's position
        transform.LookAt(player.position);

        MoveAndAttack();
    }

    // Method to handle taking damage
    public virtual void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0); //This so that it will not go below 0
        healthSlider.value = health; 
        Debug.Log("EnemyTakeDamage");

        if (health == 0)
        {
            Die(); 
        }
    }

    // Method to handle enemy death
    public virtual void Die()
    {
        Destroy(gameObject); 
    }

    public void MoveAndAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance to the player

        bool PlayerInSight = distanceToPlayer <= sightRange;
        bool PlayerNearEnemy = distanceToPlayer <= stoppingDistance;
        if (PlayerInSight && !PlayerNearEnemy)
        {
            // If the player is outside the stopping distance, move towards the player
            Vector3 directionToPlayer = player.position - transform.position; // Calculate the direction to the player
            Vector3 newPosition = player.position - directionToPlayer.normalized * stoppingDistance; // Calculate the new position to move towards
            navMeshAgent.SetDestination(newPosition); // Set the destination 

            isWalking = true; 
            enemyAnimator.SetBool("Walking", true); 
        }
        else
        {
            // If the player is outside the sight range or very close to this enemy, stop moving towards the player
            navMeshAgent.ResetPath();
            isWalking = false; 
            enemyAnimator.SetBool("Walking", false); 
        }
    }

    public virtual IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(2f); 
    }

    public void Arrest()
    {
        Debug.Log("Arrested"); 
        Destroy(gameObject); 
    }
}