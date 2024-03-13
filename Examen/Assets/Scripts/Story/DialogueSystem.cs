using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public bool isDialoguing = false;
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
        isDialoguing = true;
        dialogueFinished = false; // Reset dialogueFinished

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
        isDialoguing = false;
        dialoguePanel.SetActive(false);
        currentDialogueIndex = 0;
        dialogueFinished = true; // Indicate that dialogue is finished
    }

    public void ResetDialogue()
    {
        isDialoguing = false;
        dialoguePanel.SetActive(false);
        currentDialogueIndex = 0;
        // Other reset logic here...
    }
}
