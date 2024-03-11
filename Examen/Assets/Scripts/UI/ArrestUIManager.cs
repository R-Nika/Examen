using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrestUIManager : MonoBehaviour
{
    [SerializeField] GameObject arrestMessage;
    Transform parentObj;
    

    private void Start()
    {
        parentObj = transform.GetChild(0).transform;
    }
    public void ArrestedEnemy()
    {
        Instantiate(arrestMessage, parentObj);
    }
}
