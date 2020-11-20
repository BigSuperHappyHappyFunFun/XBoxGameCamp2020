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

        if (input.buttonUpPressed) level += $"{time},{time},Up,4\n";
        if (input.buttonDownPressed) level += $"{time},{time},Down,4\n";
        if (input.buttonLeftPressed) level += $"{time},{time},Left,4\n";
        if (input.buttonRightPressed) level += $"{time},{time},Right,4\n";

        if (queueForDebug)
            Debug.Log(level);
    }
}
