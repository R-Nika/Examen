using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    private string[] currentDialogues;
   [SerializeField] private int currentDialogueIndex;
    public bool isDialoguing = false;
    private bool eKeyPressedLastFrame = false;

    public bool dialogueFinished = false;


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

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(string[] dialogues)
    {
        dialoguePanel.SetActive(true);
        currentDialogues = dialogues;
        currentDialogueIndex = 0;  // Reset the index when starting a new dialogue
        dialogueFinished = false;
        isDialoguing = true;
        eKeyPressedLastFrame = false; // Reset the flag

        // Ensure the array is not null before accessing it
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
            currentDialogueIndex++; // Increment the index after updating the text
        }
        else
        {
            // End of dialogues
            isDialoguing = false;
            dialoguePanel.SetActive(false);

            dialogueFinished = true;
            currentDialogueIndex = 0;
        }
    }
}