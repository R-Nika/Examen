using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource ambientAudioSource;
    [SerializeField] AudioClip city, policeStation, cafe;

    private void Start()
    {
        Invoke("CheckScene", .1f);
    }

    void CheckScene()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(curSceneIndex);

        switch (curSceneIndex) {
            case 1:
                ambientAudioSource.clip = city;
                break;

            case 4:
                ambientAudioSource.clip = policeStation;
                break;

            case 5:
                ambientAudioSource.clip = cafe;
                break;
        }

        ambientAudioSource.Play();
    }
}
