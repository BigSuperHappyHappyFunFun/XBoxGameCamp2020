using UnityEngine;

public class ComboChecker : MonoBehaviour
{
    public string currentCombo = "";
    public int count = 0;

    private void OnEnable()
    {
        GameEvents.PressedCorrect.Add(CheckCombo);
        GameEvents.Failed.Add(CancelCombo);
    }

    private void CheckCombo()
    {
        var buttonRequest = FindObjectOfType<InputChecker>().closestButtonRequest;
        if (buttonRequest)
        {
            var buttonRequestCombo = buttonRequest.GetComponent<ButtonRequestCombo>();
            if (buttonRequestCombo)
                CheckCombo(buttonRequestCombo);
        }
    }

    private void CheckCombo(ButtonRequestCombo buttonRequestCombo)
    {
        if (currentCombo == "")
        {
            if (buttonRequestCombo.isStart)
            {
                count++;
                currentCombo = buttonRequestCombo.combo;
                GameEvents.LandedCombo.Invoke(count);
            }
        }
        else
        {
            count++;
            if (buttonRequestCombo.isEnd)
            {
                GameEvents.FinishedCombo.Invoke(count);
                ResetCombo();
            }
            else
            {
                GameEvents.LandedCombo.Invoke(count);
            }
        }
    }

    private void CancelCombo()
    {
        if (count > 0)
            GameEvents.CancelledCombo.Invoke(count);
        ResetCombo();
    }

    private void ResetCombo()
    {
        currentCombo = "";
        count = 0;
    }
}
