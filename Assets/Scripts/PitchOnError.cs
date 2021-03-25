using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PitchOnError : MonoBehaviour
{
    public AudioMixer mixer;
    private Coroutine coroutine;
    public float targetPitch;
    public float pitchChangeRate = 1;
    public float pitchChangeHold = 1;

    private void OnEnable()
    {
        GameEvents.Failed.Add(ChangePitch);
        mixer.SetFloat("SongPitchShifter", 1);
    }

    private void OnDisable()
    {
        GameEvents.Failed?.Remove(ChangePitch);
    }

    private void ChangePitch()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(ChangePitchCoroutine());
    }

    private IEnumerator ChangePitchCoroutine()
    {
        float pitch;
        mixer.GetFloat("SongPitchShifter", out pitch);
        while (pitch > targetPitch)
        {
            yield return null;
            pitch -= pitchChangeRate * Time.deltaTime;
            mixer.SetFloat("SongPitchShifter", pitch);
        }
        pitch = targetPitch;
        mixer.SetFloat("SongPitchShifter", pitch);
        yield return new WaitForSeconds(pitchChangeHold);
        while (pitch < 1)
        {
            yield return null;
            pitch += pitchChangeRate * Time.deltaTime;
            mixer.SetFloat("SongPitchShifter", pitch);
        }
        pitch = 1;
        mixer.SetFloat("SongPitchShifter", pitch);
        coroutine = null;
    }
}
