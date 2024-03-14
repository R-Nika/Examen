using UnityEngine;
using TMPro;

//
//
// Created/Written by: Amy van Oosten
//
//

public class DialogueSystem : MonoBehaviour
{
    public bool isInDialogue = false;
    public bool dialogueFinished = false;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    private string[] currentDialogues;
    private int currentDialogueIndex;

    private void Start()
    {     
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogues)
    {       
        dialoguePanel.SetActive(true);
        
        currentDialogues = dialogues;
        currentDialogueIndex = 0;

        isInDialogue = true;
        dialogueFinished = false; 

        // Display the first dialogue line if there are dialogues available
        if (currentDialogues != null && currentDialogues.Length > 0)
        {
            dialogueText.text = currentDialogues[currentDialogueIndex];
        }
        else
        {          
            Debug.LogWarning("Dialogue array is null or empty.");
        }
    }

    public void ContinueDialogue()
    {      
        currentDialogueIndex++;

        // If there are more dialogues, display the next one; otherwise, end the dialogue
        if (currentDialogueIndex < currentDialogues.Length)
        {
            dialogueText.text = currentDialogues[currentDialogueIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        // Update dialogue state and deactivate dialogue panel
        isInDialogue = false;
        dialoguePanel.SetActive(false);

        // Reset dialogue index and set dialogueFinished 
        currentDialogueIndex = 0;
        dialogueFinished = true; 
    }

    public void ResetDialogue()
    {
        // Reset dialogue state and deactivate dialogue panel and reset index
        isInDialogue = false;
        dialoguePanel.SetActive(false);
        currentDialogueIndex = 0;   
    }
}