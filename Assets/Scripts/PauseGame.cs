using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool isPaused;
    public AudioSource song;

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        song.Pause();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        song.Play();
    }
}
