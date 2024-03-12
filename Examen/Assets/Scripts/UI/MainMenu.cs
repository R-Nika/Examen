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
        fadeAnim.SetTrigger("Fade");
        Invoke("CloseGame", 1.5f);
    }

    void CloseGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        fadeAnim.SetTrigger("Fade");
        Invoke("LoadMainScene", 1.5f);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ReferenceLoadMainMenu()
    {
        fadeAnim.SetTrigger("Fade");
        Invoke("LoadMainMenu", 1.5f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
