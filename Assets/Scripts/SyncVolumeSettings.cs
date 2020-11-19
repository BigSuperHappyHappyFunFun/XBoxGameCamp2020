using UnityEngine;

public class SyncVolumeSettings : MonoBehaviour
{
    public AudioSource source;
    public GameSettings settings;
    [Range(0, 1)] public float volume = 1;

    private void OnValidate()
    {
        if (!source) source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        source.volume = volume * settings.volume;
    }
}
