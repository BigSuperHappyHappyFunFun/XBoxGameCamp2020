using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeStampTool : MonoBehaviour
{
    public bool isPlaying;
    public List<AudioClip> musicOptions = new List<AudioClip>();
    public TMP_Dropdown dropdown;
    public Button playButton;
    public Button resetButton;
    public Button mainMenuButton;
    public TMP_InputField inputField;
    public Slider slider;
    public AudioSource audioSource;
    public List<TimeStamp> timeStamps = new List<TimeStamp>();
    public GameInput input;
    public TextMeshProUGUI timeDisplayText;

    private void Update()
    {
        if (audioSource.clip && audioSource.isPlaying)
        {
            slider.enabled = true;
                slider.value = audioSource.time / audioSource.clip.length;
        }
        else if (!audioSource.clip)
        {
            slider.enabled = false;
            slider.value = 0;
            timeDisplayText.text = "0.000";
        }

        if (audioSource.clip && slider)
        {
            var time = slider.value * audioSource.clip.length;
            timeDisplayText.text = time.ToString("0.000");
        }

        if (audioSource.isPlaying)
        {
            if (input.buttonUpPressed) timeStamps.Add(new TimeStamp { time = audioSource.time, button = "Up" });
            if (input.buttonDownPressed) timeStamps.Add(new TimeStamp { time = audioSource.time, button = "Down" });
            if (input.buttonLeftPressed) timeStamps.Add(new TimeStamp { time = audioSource.time, button = "Left" });
            if (input.buttonRightPressed) timeStamps.Add(new TimeStamp { time = audioSource.time, button = "Right" });

            inputField.text = "";
            foreach (var timeStamp in timeStamps.OrderBy(x => x.time))
                inputField.text += $"{timeStamp.time},{timeStamp.time},{timeStamp.button}\n";
        }
    }

    private void OnEnable()
    {
        PopulateMusicOptions();
        dropdown.onValueChanged.Add(ChangeMusic);
        playButton.onClick.Add(TogglePlayPause);
        resetButton.onClick.Add(Reset);
        mainMenuButton.onClick.Add(MainMenu);
    }

    private void PopulateMusicOptions()
    {
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData("Select..."));
        foreach (var musicOption in musicOptions)
            dropdown.options.Add(new TMP_Dropdown.OptionData(musicOption.name));
    }

    private void TogglePlayPause()
    {
        if (audioSource.isPlaying)
        {
            inputField.interactable = true;
            audioSource.Pause();
        }
        else if (audioSource.clip)
        {
            inputField.interactable = false;
            audioSource.time = slider.value * audioSource.clip.length;
            audioSource.Play();
        }
    }

    private void ChangeMusic(int i)
    {
        inputField.text = "";
        timeStamps.Clear();
        audioSource.time = 0;
        slider.value = 0;
        inputField.interactable = true;
        audioSource.Stop();
        if (i > 0)
            audioSource.clip = musicOptions[i - 1];
    }

    private void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    [Serializable]
    public class TimeStamp
    {
        public float time;
        public string button;
    }
}
