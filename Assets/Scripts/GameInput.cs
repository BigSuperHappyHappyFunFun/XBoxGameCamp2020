using UnityEngine;
using UnityEngine.Events;

public class GameInput : MonoBehaviour
{
    public bool prevButtonUp;
    public bool prevButtonDown;
    public bool prevButtonLeft;
    public bool prevButtonRight;

    public bool buttonUp;
    public bool buttonDown;
    public bool buttonLeft;
    public bool buttonRight;

    public bool VirtualButtonUp { get; set; }
    public bool VirtualButtonDown { get; set; }
    public bool VirtualButtonLeft { get; set; }
    public bool VirtualButtonRight { get; set; }

    public bool buttonUpPressed;
    public bool buttonDownPressed;
    public bool buttonLeftPressed;
    public bool buttonRightPressed;

    private void Update()
    {
        var up = Input.GetAxisRaw("Vertical") > 0;
        var down = Input.GetAxisRaw("Vertical") < 0;
        var left = Input.GetAxisRaw("Horizontal") < 0;
        var right = Input.GetAxisRaw("Horizontal") > 0;

        buttonUp = VirtualButtonUp || up;
        buttonDown = VirtualButtonDown || down;
        buttonLeft = VirtualButtonLeft || left;
        buttonRight = VirtualButtonRight || right;

        buttonUpPressed = false;
        buttonDownPressed = false;
        buttonLeftPressed = false;
        buttonRightPressed = false;

        if (buttonUp && !prevButtonUp) buttonUpPressed = true;
        if (buttonDown && !prevButtonDown) buttonDownPressed = true;
        if (buttonLeft && !prevButtonLeft) buttonLeftPressed = true;
        if (buttonRight && !prevButtonRight) buttonRightPressed = true;

        prevButtonUp = buttonUp;
        prevButtonDown = buttonDown;
        prevButtonLeft = buttonLeft;
        prevButtonRight = buttonRight;
    }
}