using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// SCRIPT GEMAAKT DOOR ROBERT
//

public class Waypoint : MonoBehaviour
{
    [SerializeField] DialogueSystem dialogueSystem;

    [Space]

    [SerializeField] Camera _mainCamera;
    [SerializeField] float _scaleFactor = 0.1f;

    [Space]

    [SerializeField] List<Transform> waypointList = new List<Transform>();

    bool check = false;
    private void Update()
    {
        Scale();

        if (dialogueSystem.isInDialogue) {
            check = true;
        }

        if(dialogueSystem.dialogueFinished && check) {
            NextWaypoint();
            check = false;
        }
    }

    private void Start()
    {
        NextWaypoint();
    }
    private void Scale()
    {
        if (_mainCamera) {
            float camHeight;
            if (_mainCamera.orthographic) {
                camHeight = _mainCamera.orthographicSize * 2;
            } else {
                float distanceToCamera = Vector3.Distance(_mainCamera.transform.position, transform.position);
                camHeight = 2.0f * distanceToCamera * Mathf.Tan(Mathf.Deg2Rad * (_mainCamera.fieldOfView * 0.5f));
            }
            float scale = (camHeight / Screen.width) * _scaleFactor;
            transform.localScale = new Vector3(scale, scale, scale);


            // Calculate the direction from the UI to the camera
            Vector3 direction = _mainCamera.transform.position - transform.position;
            direction.y = 0f; // Restrict rotation to Y-axis by setting Y-component to 0

            // Rotate the UI to face the calculated direction
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    int waypointIndex;
    void NextWaypoint()
    {
        if(waypointIndex < waypointList.Count) {
            transform.position = waypointList[waypointIndex].position;
            waypointIndex++;
        }
    }
}
