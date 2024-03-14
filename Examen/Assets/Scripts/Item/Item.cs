using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//
//
// Created/Written by: Amy van Oosten
//
//

// Enum to define different types of items
public enum ItemType
{
    HealthItem
}

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public ItemType itemtype;
    public int healthModifyer;

    public TMP_Text pressE;
    private int interactionRadius = 3;

    // Update is called once per frame
    private void Update()
    {
        PlayerInRange(); 
    }

    // Method to check if the player is in range
    private void PlayerInRange()
    {   
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);     
        foreach (Collider collider in colliders)
        {          
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player is in range of an item"); 
                pressE.enabled = true;
                return; // Exit the method early since the player is in range
            }
        }
        // If the player is not in range, set the inRange to false and disable PressE
        pressE.enabled = false;
    }
}