using UnityEngine;
using UnityEngine.UI;

public class LocalInput : MonoBehaviour
{
    public bool pressA = false;
    public bool pressB = false;
    public bool pressX = false;
    public bool pressY = false;
    public bool pressL = false;
    public bool pressR = false;

    public Button buttonA;
    public Button buttonB;
    public Button buttonX;
    public Button buttonY;
    public Button buttonL;
    public Button buttonR;

    private bool virtualA = false;
    private bool virtualB = false;
    private bool virtualX = false;
    private bool virtualY = false;
    private bool virtualL = false;
    private bool virtualR = false;

    private void Start()
    {
        buttonA.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualA = true; });
        buttonB.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualB = true; });
        buttonX.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualX = true; });
        buttonY.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualY = true; });
        buttonL.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualL = true; });
        buttonR.GetComponent<PointerDownEventTrigger>().onPointerDown.AddListener(() => { virtualR = true; });
    }

    void Update()
    {
        pressA = Input.GetKeyDown(KeyCode.A) || virtualA;
        pressB = Input.GetKeyDown(KeyCode.B) || virtualB;
        pressX = Input.GetKeyDown(KeyCode.X) || virtualX;
        pressY = Input.GetKeyDown(KeyCode.Y) || virtualY;
        pressL = Input.GetKeyDown(KeyCode.L) || virtualL;
        pressR = Input.GetKeyDown(KeyCode.R) || virtualR;
        virtualA = false;
        virtualB = false;
        virtualX = false;
        virtualY = false;
        virtualL = false;
        virtualR = false;
    }
}
