using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] int mainSceneIndex = 1;
    [SerializeField] Animator fadeAnim;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        fadeAnim.SetTrigger("Fade");
        Invoke("LoadMainScene", 1.5f);
    }

    void LoadMainScene(int sceneIndex)
    {
        SceneManager.LoadScene(1);
    }
}
