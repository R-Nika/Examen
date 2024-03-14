using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// SCRIPT GEMAAKT DOOR ROBERT
//

public class ArrestMessage : MonoBehaviour
{
    float timer = 3;
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(this.gameObject);
        }
    }
}
