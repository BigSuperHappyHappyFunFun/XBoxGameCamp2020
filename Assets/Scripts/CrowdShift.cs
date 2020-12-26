using UnityEngine;

public class CrowdShift : MonoBehaviour
{
    public Vector3 targetPosition;
    public float amount = 0.5f;
    public float speed = 10f;

    private void OnEnable()
    {
        targetPosition = transform.position;
        GameEvents.PressedGreat.Add(ShiftLeft);
        GameEvents.PressedPerfect.Add(ShiftLeft);
        GameEvents.Failed.Add(ShiftRight);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void ShiftLeft()
    {
        targetPosition.x -= amount;
    }

    public void ShiftRight()
    {
        targetPosition.x += amount;
    }
}
