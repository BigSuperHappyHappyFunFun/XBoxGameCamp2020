﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    public GameInput input;
    public List<GameObject> buttonRequests = new List<GameObject>();
    public float bestThreshold = 0.1f;
    public float betterThreshold = 0.2f;
    public float goodThreshold = 0.3f;
    public float speedAdjustmentThreshold = 0.2f;
    public Transform closestButtonRequest;
    public CharacterAnimationState characterAnimationState;

    public int bestCount = 0;
    public int betterCount = 0;
    public int goodCount = 0;
    public int wrongButtonCount = 0;
    public int missCount = 0;

    public Color goodColor = Color.yellow;
    public Color betterColor = Color.yellow;
    public Color bestColor = Color.green;
    public Color wrongColor = Color.red;
    public Color missColor = Color.red;
    
    private void Update()
    {
        var anyPressed = input.buttonUpPressed
            || input.buttonDownPressed
            || input.buttonLeftPressed
            || input.buttonRightPressed;
        closestButtonRequest = GetClosestButtonRequest();
        var speedAdjustment = 1f;
        if (closestButtonRequest)
            speedAdjustment = 1 + closestButtonRequest.GetComponent<ButtonRequestMove>().speed * speedAdjustmentThreshold;

        if (anyPressed && closestButtonRequest)
        {
            var distance = dist(closestButtonRequest, transform);
            if (distance <= bestThreshold * speedAdjustment)
            {
                if (IsCorrectButton())
                {
                    bestCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = bestColor;
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedPerfect.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    GameEvents.Failed.Invoke();
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= betterThreshold * speedAdjustment)
            {
                if (IsCorrectButton())
                {
                    betterCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = betterColor;
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedGreat.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    GameEvents.Failed.Invoke(); 
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= goodThreshold * speedAdjustment)
            {
                if (IsCorrectButton())
                {
                    goodCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = goodColor;
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedGood.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    GameEvents.Failed.Invoke();
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
        }
        for (var i = buttonRequests.Count - 1; i >= 0; i--)
        {
            var buttonRequest = buttonRequests[i];
            var tooLate = buttonRequest.transform.position.x < transform.position.x - goodThreshold * speedAdjustment;
            if (tooLate)
            {
                missCount++;
                buttonRequests.RemoveAt(i);
                buttonRequest.GetComponent<SpriteRenderer>().color = missColor;
                GameEvents.Failed.Invoke();
                GameEvents.Missed.Invoke();
            }
        }
    }

    private bool IsCorrectButton()
    {
        var name = closestButtonRequest.name;
        if (name == "arrowUp") return input.buttonUpPressed;
        if (name == "arrowDown") return input.buttonDownPressed;
        if (name == "arrowLeft") return input.buttonLeftPressed;
        if (name == "arrowRight") return input.buttonRightPressed;
        return false;
    }

    private Transform GetClosestButtonRequest()
    {
        if (buttonRequests.Count == 0)
            return null;

        var t = transform;
        return buttonRequests
            .Select(g => g.transform)
            .Aggregate((a, b) => dist(a, t) < dist(b, t) ? a : b);
    }

    private float dist(Transform a, Transform b) => Vector3.Distance(a.position, b.position);
}
