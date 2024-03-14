using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//
//
// Created/Written by: Amy van Oosten
//
//

public class StoryManager : MonoBehaviour
{
    [Header("Story Settings")]
    public GameObject[] npcs;
    public DialogueSystem dialogue; 

    private int currentNPCIndex = 0; 

    private void Start()
    {
        DeactivateAllNPCs(); // Deactivate all NPCs at the start
        ActivateCurrentNPC(); // Activate the initial NPC
    }

    private void Update()
    {
        // Check if not in dialogue or dialogue has finished, and 'E' key is pressed
        if ((!dialogue.isInDialogue || dialogue.dialogueFinished) && Input.GetKeyDown(KeyCode.E))
        {
            currentNPCIndex++; 

            if (currentNPCIndex < npcs.Length) 
            {
                ActivateCurrentNPC(); 
            }
            else
            {
                Debug.Log("All NPCs are done!"); 
            }
        }

        // Check if dialogue has finished, then reset the dialogue system
        if (dialogue.dialogueFinished)
        {
            dialogue.ResetDialogue(); 
            Debug.Log("Dialogue system reset."); 
        }
    }

    private void ActivateCurrentNPC()
    {
        npcs[currentNPCIndex].SetActive(true);
        npcs[currentNPCIndex].GetComponent<NPC>().enabled = true; 
    }

    private void DeactivateAllNPCs()
    {
        foreach (GameObject npc in npcs)
        {
            npc.SetActive(false); 
            npc.GetComponent<NPC>().enabled = false;
        }
    }
}