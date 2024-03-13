using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

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
        transform.LookAt(player.position);

        MoveAndAttack();
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;
        Debug.Log("EnemyTakeDamage");
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
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
                isWalking = false;
                enemyAnimator.SetBool("Walking", false);
            }
            else
            {
                Vector3 directionToPlayer = player.position - transform.position;
                Vector3 newPosition = player.position - directionToPlayer.normalized * stoppingDistance;
                navMeshAgent.SetDestination(newPosition);

               

                isWalking = true;
                enemyAnimator.SetBool("Walking", true);
            }
        }
        else
        {
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