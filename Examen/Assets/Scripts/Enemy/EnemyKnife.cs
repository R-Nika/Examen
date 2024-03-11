using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : Enemy
{
    [Header("Enemy Knife Settings")]
    public float attackInterval = 2f; 

    private float knifeRange = 3;
    private int knifeDamage = 5;
    private bool isAttacking = false;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

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
        player.GetComponent<Player>().TakeDamage(knifeDamage);
    }
}
