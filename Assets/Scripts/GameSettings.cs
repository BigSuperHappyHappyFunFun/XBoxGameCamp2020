using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    public int majorVersion = 0;
    public int minorVersion = 0;
    public int microVersion = 0;
    [SerializeField] private float _volume;
    internal bool showTouchControls;

    public string Version => $"{majorVersion}.{minorVersion}.{microVersion}";
    public bool IsMute { get; set; }

    public float Volume
    {
        get => IsMute ? 0 : _volume;
        set => _volume = value;
    }

    public float VolumeIgnoreMute => _volume;

    public void Awake()
    {
        if (PlayerPrefs.HasKey("IsMute"))
            IsMute = PlayerPrefs.GetInt("IsMute") > 0;
        if (PlayerPrefs.HasKey("Volume"))
            _volume = PlayerPrefs.GetFloat("Volume");
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetInt("IsMute", IsMute ? 1 : 0);
        PlayerPrefs.SetFloat("Volume", _volume);
        PlayerPrefs.Save();
    }
}