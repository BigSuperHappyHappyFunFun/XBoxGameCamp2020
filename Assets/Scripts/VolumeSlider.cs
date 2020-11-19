using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public GameSettings settings;
    public Slider slider;

    private void OnValidate()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        slider.value = settings.volume;
        slider.onValueChanged.AddListener(AdjustVolume);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(AdjustVolume);
    }

    private void AdjustVolume(float sliderValue)
    {
        settings.volume = sliderValue;
    }
}
