using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    void Start()
    {
        foreach (var levelData in FindObjectsOfType<LevelData>())
            foreach (var buttonDisplay in FindObjectsOfType<ButtonDisplay>())
                StartCoroutine(DisplayButton(levelData, buttonDisplay));
    }

    IEnumerator DisplayButton(LevelData levelData, ButtonDisplay buttonDisplay)
    {
        var time = 0f;
        for (var i = 0; i < levelData.buttonRequests.Count; i = i)
        {
            var buttonRequest = levelData.buttonRequests[i];
            if (buttonRequest.start < time)
            {
                buttonDisplay.spriteRenderer.sprite = GetSprite(buttonDisplay, buttonRequest);
                i++;
            }
            yield return null;
            time += Time.deltaTime;
        }
    }

    Sprite GetSprite(ButtonDisplay buttonDisplay, LevelData.ButtonRequest buttonRequest)
    {
        switch (buttonRequest.button.Trim())
        {
            case "A": return buttonDisplay.ButtonA;
            case "B": return buttonDisplay.ButtonB;
            case "X": return buttonDisplay.ButtonX;
            case "Y": return buttonDisplay.ButtonY;
            case "L": return buttonDisplay.ButtonL;
            case "R": return buttonDisplay.ButtonR;
            default: return null;
        }
    }
}
