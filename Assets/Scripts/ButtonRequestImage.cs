using UnityEngine;

public class ButtonRequestImage : MonoBehaviour
{
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject leftArrow;
    public GameObject rightArrow;

    private void OnEnable()
    {
        // upArrow?.SetActive(false);
        // downArrow?.SetActive(false);
        // leftArrow?.SetActive(false);
        // rightArrow?.SetActive(false);
    }

    public void ShowArrow(string direction)
    {
        if (direction.ToLower() == "up") upArrow?.SetActive(true);
        if (direction.ToLower() == "down") downArrow?.SetActive(true);
        if (direction.ToLower() == "left") leftArrow?.SetActive(true);
        if (direction.ToLower() == "right") rightArrow?.SetActive(true);
    }

    public void SetColor(Color color)
    {
        upArrow.GetComponent<SpriteRenderer>().color = color;
        downArrow.GetComponent<SpriteRenderer>().color = color;
        leftArrow.GetComponent<SpriteRenderer>().color = color;
        rightArrow.GetComponent<SpriteRenderer>().color = color;
    }
}
