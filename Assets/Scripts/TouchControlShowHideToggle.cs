using UnityEngine;
using UnityEngine.UI;

public class TouchControlShowHideToggle : MonoBehaviour
{
    public GameSettings gameSettings;
    public Toggle toggle;

    private void OnValidate()
    {
        if (!toggle) toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        toggle.isOn = gameSettings.showTouchControls;
        toggle.onValueChanged.Add(UpdateSettings);
    }

    private void UpdateSettings(bool value)
    {
        gameSettings.showTouchControls = value;
    }
}