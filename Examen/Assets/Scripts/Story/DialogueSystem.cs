using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public bool dialogueFinished = false;
    public bool isDialoguing = false;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    private string[] currentDialogues;
    private int currentDialogueIndex;
    private bool eKeyPressedLastFrame = false;
  
    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogues)
    {
        dialoguePanel.SetActive(true);
        currentDialogues = dialogues;
        currentDialogueIndex = 0; 
        dialogueFinished = false;
        isDialoguing = true;
        eKeyPressedLastFrame = false; 

        if (currentDialogues != null && currentDialogues.Length > 0)
        {
            dialogueText.text = currentDialogues[currentDialogueIndex];
        }
        else
        {
            Debug.LogWarning("Dialogue array is null or empty.");
        }
    }

    void Update()
    {
        if (isDialoguing)
        {
            if (Input.GetKeyDown(KeyCode.E) && !eKeyPressedLastFrame)
            {
                ContinueDialogue();
            }
        }
        eKeyPressedLastFrame = Input.GetKey(KeyCode.E);
    }

    void ContinueDialogue()
    {
        if (currentDialogueIndex < currentDialogues.Length)
        {
            dialogueText.text = currentDialogues[currentDialogueIndex];
            currentDialogueIndex++; 
        }
        else
        {           
            isDialoguing = false;
            dialoguePanel.SetActive(false);
            dialogueFinished = true;
            currentDialogueIndex = 0;
        }
    }
}