using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRecorder : MonoBehaviour
{
    public GameInput input;

    public string level;
    public float time;

    private void Update()
    {
        time += Time.deltaTime;
        var queueForDebug = false;
        if (input.buttonDownPressed || input.buttonLeftPressed || input.buttonRightPressed || input.buttonUpPressed)
            queueForDebug = true;

        if (input.buttonUpPressed) level += $"{time},up\n";
        if (input.buttonDownPressed) level += $"{time},down\n";
        if (input.buttonLeftPressed) level += $"{time},left\n";
        if (input.buttonRightPressed) level += $"{time},right\n";

        if (queueForDebug)
            Debug.Log(level);
    }
}
