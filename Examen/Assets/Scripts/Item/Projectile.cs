using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public int baseDamage = 1;
    private Transform player;

    void Start()
    {
        Rigidbody bulletRb = GetComponent<Rigidbody>();
        bulletRb.mass = 0.01f; 
        bulletRb.drag = 0.01f;
        bulletRb.useGravity = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                int modifiedDamage = CalculateModifiedDamage();
                enemy.TakeDamage(modifiedDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Player"))
        {
            Player hitPlayer = collision.gameObject.GetComponent<Player>();
            if (hitPlayer != null)
            {
                if (hitPlayer != player)
                {
                    int modifiedDamage = CalculateModifiedDamage();
                    hitPlayer.TakeDamage(modifiedDamage);
                }
            }
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }

    private int CalculateModifiedDamage()
    {        
        int additionalDamage = player.GetComponent<Player>().currency / 10;
        return baseDamage + additionalDamage;
    }
}