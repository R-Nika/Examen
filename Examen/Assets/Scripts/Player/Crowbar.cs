using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// Created/Written by: Amy van Oosten
//
//

public class Crowbar : MonoBehaviour
{
    [Header("Crowbar Settings")]
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
