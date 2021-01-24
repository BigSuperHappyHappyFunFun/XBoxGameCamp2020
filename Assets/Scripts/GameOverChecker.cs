using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverChecker : MonoBehaviour
{
    public int totalNotes = 12;
    public int loseNotes = 3;
    public int currentFailedNotes = 0;
    public float failPercent = 0.25f;
    public LevelData levelData;

    private void OnEnable()
    {
        GameEvents.LevelStarted.Add(InitializeValues);
        GameEvents.Failed.Add(AddCurrentFailedNotes);
    }

    private void OnDisable()
    {
        GameEvents.LevelStarted?.Remove(InitializeValues);
        GameEvents.Failed?.Remove(AddCurrentFailedNotes);
    }

    private void InitializeValues()
    {
        totalNotes = levelData.buttonRequests.Count;
        loseNotes = Mathf.CeilToInt(totalNotes * failPercent);
        currentFailedNotes = 0;
    }

    private void AddCurrentFailedNotes()
    {
        currentFailedNotes++;
        if (currentFailedNotes >= loseNotes)
            GameEvents.GameOver.Invoke();
    }
}
