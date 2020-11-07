using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AnimationCurve volumeCurve;
    public GameSettings settings;
    public AudioMixer mixer;
    public Slider slider;
    public float volumeMinDB = -80f;

    private void OnValidate()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        slider.value = settings.volume;
        AdjustVolume(slider.value);
        slider.onValueChanged.AddListener(AdjustVolume);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(AdjustVolume);
    }

    private void AdjustVolume(float sliderValue)
    {
        settings.volume = sliderValue;
        var volume2db = volumeMinDB + (volumeCurve.Evaluate(sliderValue) * -volumeMinDB);
        mixer.SetFloat("MasterVolume", volume2db);
    }
}
