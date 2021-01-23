using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsShowHide : MonoBehaviour
{
    public GameSettings gameSettings;
    public List<GameObject> touchControlButtons = new List<GameObject>();

    private void Update()
    {
        foreach (var button in touchControlButtons)
            button.SetActive(gameSettings.showTouchControls);
    }
}
