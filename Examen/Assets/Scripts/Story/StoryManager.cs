using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class StoryManager : MonoBehaviour
{
    [Header("Story Settings")]
    public GameObject[] npcs;
    public DialogueSystem dialogue;

    private int currentNPCIndex = 0;

    private void Start()
    {
        DeactivateAllNPCs();
        ActivateCurrentNPC();
    }

    private void Update()
    {
        if ((dialogue.isDialoguing == false || dialogue.dialogueFinished) && Input.GetKeyDown(KeyCode.E))
        {
            //npcs[currentNPCIndex].GetComponent<NPC>().ResetInteraction();
            currentNPCIndex++;

            if (currentNPCIndex < npcs.Length)
            {
                ActivateCurrentNPC();
            }
            else
            {
                Debug.Log("All NPCs are done!");
                // Optionally, you can reset the cycle by uncommenting the line below
                // currentNPCIndex = 0;
            }
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
