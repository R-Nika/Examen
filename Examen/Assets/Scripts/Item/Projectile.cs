using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 1;

    void Start()
    {
        Rigidbody bulletRb = this.GetComponent<Rigidbody>();
        bulletRb.mass = 0.01f; // Experiment with different values
        bulletRb.drag = 0.01f; // Experiment with different values
        bulletRb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
      

        if (collision.transform.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        if (collision.transform.CompareTag("Player"))
        {
            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            //  Debug.Log(player);
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.GetComponent<Player>().TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}

