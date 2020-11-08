using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static GameObject singleton;

    public GameSettings settings;

    [RuntimeInitializeOnLoadMethod]
    public static void MainCreate()
    {
        if (singleton) return;
        singleton = Instantiate(Resources.Load<GameObject>("Main"));
        singleton.name = "Main";
        DontDestroyOnLoad(singleton);
    }

    private void Awake() => settings.Awake();
    private void OnDestroy() => settings.OnDestroy();

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
