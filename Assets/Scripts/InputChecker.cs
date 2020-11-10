using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    public LocalInput localInput;
    public List<GameObject> buttonRequests = new List<GameObject>();
    public float bestThreshold = 0.1f;
    public float betterThreshold = 0.2f;
    public float goodThreshold = 0.3f;
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
    
    private bool pressA;
    private bool pressB;
    private bool pressX;
    private bool pressY;
    private bool pressL;
    private bool pressR;
    private bool pressAny;

    private void Update()
    {
        pressA = localInput.pressA;
        pressB = localInput.pressB;
        pressX = localInput.pressX;
        pressY = localInput.pressY;
        pressL = localInput.pressL;
        pressR = localInput.pressR;
        pressAny = pressA || pressB || pressX || pressY || pressL || pressR;
        closestButtonRequest = GetClosestButtonRequest();

        if (pressAny && closestButtonRequest)
        {
            var distance = dist(closestButtonRequest, transform);
            if (distance <= bestThreshold)
            {
                if (IsCorrectButton())
                {
                    bestCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = bestColor;
                    characterAnimationState.triggerSuccess = true;
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    characterAnimationState.triggerFail = true;
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= betterThreshold)
            {
                if (IsCorrectButton())
                {
                    betterCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = betterColor;
                    characterAnimationState.triggerSuccess = true;
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    characterAnimationState.triggerFail = true;
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
            else if (distance <= goodThreshold)
            {
                if (IsCorrectButton())
                {
                    goodCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = goodColor;
                    characterAnimationState.triggerSuccess = true;
                }
                else
                {
                    wrongButtonCount++;
                    closestButtonRequest.GetComponent<SpriteRenderer>().color = wrongColor;
                    characterAnimationState.triggerFail = true;
                }
                buttonRequests.Remove(closestButtonRequest.gameObject);
            }
        }
        for (var i = buttonRequests.Count - 1; i >= 0; i--)
        {
            var buttonRequest = buttonRequests[i];
            var tooLate = buttonRequest.transform.position.x < transform.position.x - goodThreshold;
            if (tooLate)
            {
                missCount++;
                buttonRequests.RemoveAt(i);
                buttonRequest.GetComponent<SpriteRenderer>().color = missColor;
                characterAnimationState.triggerFail = true;
            }
        }
    }

    private bool IsCorrectButton()
    {
        var name = closestButtonRequest.name;
        if (name == "buttonA") return pressA;
        if (name == "buttonB") return pressB;
        if (name == "buttonX") return pressX;
        if (name == "buttonY") return pressY;
        if (name == "buttonL") return pressL;
        if (name == "buttonR") return pressR;
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
