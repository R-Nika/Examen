using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string[] npcDialogues;
    public float interactionRadius = 3f; // Adjust the radius as needed
    public TMP_Text pressE;
    public DialogueSystem dialogue;

    private bool interactionTriggered = false;
    private bool inRange = false;

    [Header("NPC Settings")]
    [HideInInspector] public bool farmerActive = false;
    [HideInInspector] public bool cafeActive = false;
    [HideInInspector] public bool phoneActive = false;
    [HideInInspector] public bool policeActive = false;

    [Header("Audio Settings")]
    public AudioSource audioNPC;

    private void Start()
    {
        pressE.enabled = false;
    }

    private void Update()
    {
        CheckPlayerInRange();
        if (inRange)
        {
            pressE.enabled = true;
        }
        else
        {
            pressE.enabled = false;
        }
    }

    private void CheckPlayerInRange()
    {
        if (!interactionTriggered)
        {          
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    inRange = true;

                    if (Input.GetButtonDown("Interact"))
                    {
                        dialogue.StartDialogue(npcDialogues);
                        audioNPC.Stop();
                        SetActiveFlag();
                        interactionTriggered = true; 
                    }
                    return; 
                }
            } 
            inRange = false;
        }
    }

    private void SetActiveFlag()
    {
        switch (gameObject.tag)
        {
            case "Phone":
                phoneActive = true;
                break;
            case "Farmer":
                farmerActive = true;
                break;
            case "Cafe":
                cafeActive = true;
                
                break;
            case "Police":
                policeActive = true;
                break;
        }
    }

    public void ResetInteraction()
    {
        interactionTriggered = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
