using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public GameSettings settings;
    public Slider slider;
    public bool isMute;
    public Button speakerButton;
    public Sprite speakerMuteImage;
    public Sprite speakerImage;

    private void OnValidate()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        slider.value = settings.VolumeIgnoreMute;
        slider.onValueChanged.AddListener(AdjustVolume);
        if (speakerButton) speakerButton.onClick.Add(ToggleMute);
        if (speakerButton) UpdateSpeakerIcon();
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(AdjustVolume);
        if (speakerButton) speakerButton.onClick.Remove(ToggleMute);
    }

    private void AdjustVolume(float sliderValue)
    {
        settings.Volume = sliderValue;
    }

    private void ToggleMute()
    {
        settings.IsMute = !settings.IsMute;
        UpdateSpeakerIcon();
    }

    private void UpdateSpeakerIcon()
    {
        var image = speakerButton.GetComponent<Image>();
        image.sprite = settings.IsMute ? speakerMuteImage : speakerImage;
    }
}
