using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public GameInput input;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = "";
        if (input.buttonUp) textMesh.text += "Up\n";
        if (input.buttonDown) textMesh.text += "Down\n";
        if (input.buttonLeft) textMesh.text += "Left\n";
        if (input.buttonRight) textMesh.text += "Right\n";
        if (textMesh.text == "") textMesh.text = "None";
    }
}
