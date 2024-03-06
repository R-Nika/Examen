using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour
{
    private int range = 3;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseCrowbar();
        }
    }

    public void UseCrowbar()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("SecretDoor"))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}
