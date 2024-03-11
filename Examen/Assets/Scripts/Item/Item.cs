using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
