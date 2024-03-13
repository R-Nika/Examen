using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
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
        SceneManager.LoadScene(4);
    }

    public void ReferenceLoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
