using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//
// SCRIPT GEMAAKT DOOR ROBERT
//

public class FinalSceneManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(6);
        }
    }
}
