using UnityEngine;

public class StartSongAt : MonoBehaviour
{
    [SerializeField] private float startTime;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.time = startTime;
    }
}
