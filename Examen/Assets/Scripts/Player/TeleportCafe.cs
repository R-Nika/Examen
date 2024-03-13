using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCafe : MonoBehaviour
{
    [SerializeField] Transform teleportPos;
    [SerializeField] GameObject rainObj;
    [SerializeField] AudioSource cafeAudio, cafeMusic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.transform.position = teleportPos.position;
            rainObj.SetActive(!rainObj.activeInHierarchy);
            cafeAudio.enabled = !cafeAudio.isActiveAndEnabled;
            cafeMusic.enabled = !cafeMusic.isActiveAndEnabled;
        }
    }

}
