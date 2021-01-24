using UnityEngine;

public class DeactivateOnGameOver : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.GameOver.Add(Deactivate);
    }

    private void OnDisable()
    {
        GameEvents.GameOver.Remove(Deactivate);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
