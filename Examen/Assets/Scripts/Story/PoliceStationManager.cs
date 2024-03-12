using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoliceStationManager : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;
    [SerializeField] Animator fadeAnimator;
    bool check = true;
    private void Update()
    {
        if (dialogueSystem.dialogueFinished && check) {

            fadeAnimator.SetTrigger("Fade");
            Invoke("LoadScene", 1.5f);
            check = false;
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
