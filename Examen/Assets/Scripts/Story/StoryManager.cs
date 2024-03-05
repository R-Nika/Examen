using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class StoryManager : MonoBehaviour
{
    public GameObject npcphone;
    public GameObject npcfarmer;
    public GameObject npccafe;
    public GameObject npcpolice;

    public DialogueSystem dialogue;


    private void Start()
    {
        npcphone.SetActive(true);
        npcfarmer.SetActive(false);
        npccafe.SetActive(false);
        npcpolice.SetActive(false);
    }

    private void Update()
    {
        NPCManager();
    }

   public void NPCManager()
{
    if (dialogue.isDialoguing == false || dialogue.dialogueFinished)
    {
        if (npcphone.GetComponent<NPC>().phoneActive && dialogue.dialogueFinished)
        {
            npcfarmer.SetActive(true);
            npcphone.SetActive(false);
            npcphone.GetComponent<NPC>().ResetInteraction(); // Reset interaction for the phone NPC
        }
        if (npcfarmer.GetComponent<NPC>().farmerActive && dialogue.dialogueFinished)
        {
            npccafe.SetActive(true);
            npcfarmer.SetActive(false);
            npcfarmer.GetComponent<NPC>().ResetInteraction(); // Reset interaction for the farmer NPC
        }
        if (npccafe.GetComponent<NPC>().cafeActive && dialogue.dialogueFinished)
        {
            npcpolice.SetActive(true);
            npccafe.SetActive(false);
            npccafe.GetComponent<NPC>().ResetInteraction(); // Reset interaction for the cafe NPC
        }
        if (npcpolice.GetComponent<NPC>().policeActive && dialogue.dialogueFinished)
        {
            npcpolice.SetActive(false);
            npcpolice.GetComponent<NPC>().ResetInteraction(); // Reset interaction for the police NPC
        }
    }
}
}
