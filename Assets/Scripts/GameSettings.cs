using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : ScriptableObject
{
    public int majorVersion = 0;
    public int minorVersion = 0;
    public int microVersion = 0;
    public float volume;
    internal bool showTouchControls;

    public string Version => $"{majorVersion}.{minorVersion}.{microVersion}";

    public void Awake()
    {
        if (PlayerPrefs.HasKey("Volume"))
            volume = PlayerPrefs.GetFloat("Volume");
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
