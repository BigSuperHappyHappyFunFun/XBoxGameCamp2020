using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public bool VirtualButtonUp { get; set; }
    public bool VirtualButtonDown { get; set; }
    public bool VirtualButtonLeft { get; set; }
    public bool VirtualButtonRight { get; set; }

    public bool buttonUp;
    public bool buttonDown;
    public bool buttonLeft;
    public bool buttonRight;

    public TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        var up = Input.GetAxisRaw("Vertical") > 0;
        var down = Input.GetAxisRaw("Vertical") < 0;
        var left = Input.GetAxisRaw("Horizontal") < 0;
        var right = Input.GetAxisRaw("Horizontal") > 0;

        buttonUp = VirtualButtonUp || up;
        buttonDown = VirtualButtonDown || down;
        buttonLeft = VirtualButtonLeft || left;
        buttonRight = VirtualButtonRight || right;

        textMesh.text = "";
        if (buttonUp) textMesh.text += "Up\n";
        if (buttonDown) textMesh.text += "Down\n";
        if (buttonLeft) textMesh.text += "Left\n";
        if (buttonRight) textMesh.text += "Right\n";
        if (textMesh.text == "") textMesh.text = "None";
    }
}
