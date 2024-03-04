using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text dialogueText;
    private string[] currentDialogues;
   [SerializeField] private int currentDialogueIndex;
    private bool isDialoguing = false;
    private bool eKeyPressedLastFrame = false;

    // Singleton pattern
    public static DialogueSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(string[] dialogues)
    {
        currentDialogues = dialogues;
       // currentDialogueIndex = 0; // Reset index when starting a new dialogue
        isDialoguing = true;
        eKeyPressedLastFrame = false; // Reset the flag
    }

    void Update()
    {
        // Check for 'E' key outside of the Update method
        if (isDialoguing)
        {
            if (Input.GetKeyDown(KeyCode.E) && !eKeyPressedLastFrame)
            {
                ContinueDialogue();
            }
        }

        // Update the flag for the next frame
        eKeyPressedLastFrame = Input.GetKey(KeyCode.E);
    }

    void ContinueDialogue()
    {
        if (currentDialogueIndex < currentDialogues.Length)
        {
            dialogueText.text = currentDialogues[currentDialogueIndex];
            currentDialogueIndex++;

            Debug.Log(currentDialogueIndex);
            Debug.Log("It works");
        }
        else
        {
            // End of dialogues
            isDialoguing = false;
            dialogueText.text = "End of dialogues";
            currentDialogueIndex = 0;
        }
    }
}