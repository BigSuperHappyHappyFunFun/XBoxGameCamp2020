using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    public Button button;

    private void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.Add(GoToNextLevel);
        if (SceneManager.GetActiveScene().name == "Level4") button.GetComponentInChildren<TextMeshProUGUI>().text = "You Win!";
    }

    private void OnDisable()
    {
        button.onClick.Remove(GoToNextLevel);
    }

    private void GoToNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level1") SceneManager.LoadScene("Level2");
        if (SceneManager.GetActiveScene().name == "Level2") SceneManager.LoadScene("Level3");
        if (SceneManager.GetActiveScene().name == "Level3") SceneManager.LoadScene("Ending");
    }
}
