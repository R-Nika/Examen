using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCursor : MonoBehaviour
{
    //
    // SCRIPT GEMAAKT DOOR ROBERT
    //
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
