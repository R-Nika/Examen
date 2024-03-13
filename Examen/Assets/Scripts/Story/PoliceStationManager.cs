using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceStationManager : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] Animator fadeAnimator;
    [SerializeField] GameObject missionBoardText;
    [SerializeField] BoxCollider exitTrigger;
    [SerializeField] AudioSource phone;
    bool check = true;
    private void Update()
    {
        if (dialogueSystem.isDialoguing) {
            phone.Stop();
        }

        if (dialogueSystem.dialogueFinished && check) {
            missionBoardText.SetActive(true);
            exitTrigger.enabled = true;
            check = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && dialogueSystem.dialogueFinished) {
            fadeAnimator.SetTrigger("Fade");
            Invoke("LoadScene", 1.5f);
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
