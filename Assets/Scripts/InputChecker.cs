using System.Collections.Generic;
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
    public Animator animator;
    public TargetSpriteAnimate targetSpriteAnimate;

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

    private void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!targetSpriteAnimate) targetSpriteAnimate = transform.parent.GetComponentInChildren<TargetSpriteAnimate>();
    }

    private void Update()
    {
        var anyPressed = input.buttonUpPressed
            || input.buttonDownPressed
            || input.buttonLeftPressed
            || input.buttonRightPressed;
        closestButtonRequest = GetClosestButtonRequest();

        var xExtent = transform.lossyScale.x / 2;

        if (closestButtonRequest)
        {
            var distance = dist(closestButtonRequest, transform);
            animator.SetBool("IsReady", distance < xExtent * 1.1f);
            targetSpriteAnimate.PlayAnimation(closestButtonRequest.name);
        }
        else
        {
            animator.SetBool("IsReady", false);
            targetSpriteAnimate.PlayAnimation("None");
        }

        if (anyPressed && closestButtonRequest)
        {
            var distance = dist(closestButtonRequest, transform);
            if (distance <= bestThreshold * xExtent)
            {
                if (IsCorrectButton())
                {
                    bestCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(bestColor);
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedPerfect.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(wrongColor);
                    GameEvents.Failed.Invoke();
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= betterThreshold * xExtent)
            {
                if (IsCorrectButton())
                {
                    betterCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(betterColor);
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedGreat.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(wrongColor);
                    GameEvents.Failed.Invoke(); 
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= goodThreshold * xExtent)
            {
                if (IsCorrectButton())
                {
                    goodCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(goodColor);
                    GameEvents.PressedCorrect.Invoke();
                    GameEvents.PressedGood.Invoke();
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<ButtonRequestImage>().SetColor(wrongColor);
                    GameEvents.Failed.Invoke();
                    GameEvents.PressedWrong.Invoke();
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
        }
        for (var i = buttonRequests.Count - 1; i >= 0; i--)
        {
            var buttonRequest = buttonRequests[i];
            var tooLate = buttonRequest.transform.position.x < transform.position.x - goodThreshold * xExtent;
            if (tooLate)
            {
                missCount++;
                buttonRequests.RemoveAt(i);
                buttonRequest.GetComponent<ButtonRequestImage>().SetColor(missColor);
                GameEvents.Failed.Invoke();
                GameEvents.Missed.Invoke();
            }
        }
    }

    private bool IsCorrectButton()
    {
        var name = closestButtonRequest.name;
        if (name == "Up") return input.buttonUpPressed;
        if (name == "Down") return input.buttonDownPressed;
        if (name == "Left") return input.buttonLeftPressed;
        if (name == "Right") return input.buttonRightPressed;
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
