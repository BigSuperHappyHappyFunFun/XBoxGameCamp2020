using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartLevelButton : MonoBehaviour
{
    public Button button;

    private void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.Add(RestartLevel);
    }

    private void OnDisable()
    {
        button.onClick.Remove(RestartLevel);
    }

    private void RestartLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level1") SceneManager.LoadScene("Level1");
        if (SceneManager.GetActiveScene().name == "Level2") SceneManager.LoadScene("Level2");
        if (SceneManager.GetActiveScene().name == "Level3") SceneManager.LoadScene("Level3");
        if (SceneManager.GetActiveScene().name == "Level4") SceneManager.LoadScene("Level4");
    }
}