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

    private bool inRange = false;

    //[Header("Audio Settings")]
    // public AudioSource audioNPC;

    private void Start()
    {
        pressE.enabled = false;
    }

    private void Update()
    {
        CheckPlayerInRange();
        if (inRange /*&& !dialogue.isDialoguing*/) // Check if dialogue is not active to avoid overlapping dialogues
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                inRange = true;

                if (Input.GetButtonDown("Interact"))
                {
                    StartInteraction();
                }
                if (dialogue.dialogueContinue && dialogue.isDialoguing)
                {
                    dialogue.ContinueDialogue();
                }
                return;
            }
        }
        inRange = false;
    }

    private void StartInteraction()
    {
        dialogue.StartDialogue(npcDialogues);

        //audioNPC.Stop();
    }

    //public void ResetInteraction()
    //{
    //    inRange = false; // Reset inRange flag to allow interaction again
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}