using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AnimationCurve volumeCurve;
    public AudioMixer mixer;
    public Slider slider;
    public float volumeMinDB = -80f;

    private void OnValidate()
    {
        if (!slider) slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(AdjustVolume);
        AdjustVolume(slider.value);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(AdjustVolume);
    }

    private void AdjustVolume(float newVolume)
    {
        var volume2db = volumeMinDB + (volumeCurve.Evaluate(newVolume) * -volumeMinDB);
        mixer.SetFloat("MasterVolume", volume2db);
    }
}
