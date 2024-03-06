using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Base damage without currency influence
    public int baseDamage = 1;

    // Reference to the player to access its currency
    private Transform player;

    void Start()
    {
        Rigidbody bulletRb = GetComponent<Rigidbody>();
        bulletRb.mass = 0.01f; // Experiment with different values
        bulletRb.drag = 0.01f; // Experiment with different values
        bulletRb.useGravity = false;

        // Find the player object and get the Player script
      
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
                // Calculate damage based on currency
                int modifiedDamage = CalculateModifiedDamage();
                enemy.TakeDamage(modifiedDamage);
            }
        }
        else if (collision.transform.CompareTag("Player"))
        {
            Player hitPlayer = collision.gameObject.GetComponent<Player>();
            if (hitPlayer != null)
            {
                // Player should not take damage from their own projectile
                if (hitPlayer != player)
                {
                    // Calculate damage based on currency
                    int modifiedDamage = CalculateModifiedDamage();
                    hitPlayer.TakeDamage(modifiedDamage);
                }
            }
        }

        Destroy(gameObject);
    }

    // Calculate modified damage based on player's currency
    private int CalculateModifiedDamage()
    {
        // For every 10 dollars, add 0.5 to damage
        int additionalDamage = player.GetComponent<Player>().currency / 10;
        return baseDamage + additionalDamage;
    }
}