using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private bool inRange = false;
    private int interactionRadius = 3;

    private void Update()
    {
        PlayerInRange();
    }

    void PlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player is in range of an NPC");
                pressE.enabled = true;
                inRange = true;
                return;
            }
        }
        inRange = false;
        pressE.enabled = false;
    }
}
