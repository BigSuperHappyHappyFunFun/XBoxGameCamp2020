using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCharacterAnimation : MonoBehaviour
{
    public KeyCode success = KeyCode.Q;
    public KeyCode fail = KeyCode.E;

    private void Update()
    {
        if (Input.GetKeyDown(success))
            GameEvents.PressedCorrect.Invoke();
        if (Input.GetKeyDown(fail))
            GameEvents.Failed.Invoke();
    }
}
