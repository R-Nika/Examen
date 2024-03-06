using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnife : Enemy
{

    public float attackInterval = 2f; // Time interval between knife attacks
    private float knifeRange = 3;

    private bool isAttacking = false;

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
        // Implement your knife attack logic here
        Debug.Log("Knife attack!");
        player.GetComponent<Player>().TakeDamage(damage);
        // You can add code here to deal damage to the player or trigger any other actions
    }
}
