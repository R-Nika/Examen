using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// Created/Written by: Amy van Oosten
//
//

public class StabEnemy : Enemy
{
    [Header("Enemy Knife Settings")]
    public float attackInterval = 2f; 

    private float knifeRange = 1;

    private int knifeDamage = 5;
    private bool isAttacking = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start(); 
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update(); 

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
      
        if (distanceToPlayer <= knifeRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }      
        else if (distanceToPlayer > knifeRange && isAttacking)
        {
            StopCoroutine(AttackRoutine()); 
            isAttacking = false; 
        }
        else
        {
            enemyAnimator.SetBool("Knife", false); 
        }
    }

    public override IEnumerator AttackRoutine()
    {
        isAttacking = true; 
        while (isAttacking)
        {
            yield return new WaitForSeconds(attackInterval); 
            KnifeAttack(); 
        }
    }

    private void KnifeAttack()
    {
        enemyAnimator.SetBool("Knife", true); 
        player.GetComponent<Player>().TakeDamage(knifeDamage); // Deal damage to the player
    }
}