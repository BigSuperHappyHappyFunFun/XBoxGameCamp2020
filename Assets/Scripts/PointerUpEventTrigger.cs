using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerUpEventTrigger : MonoBehaviour, IPointerUpHandler
{
    public UnityEvent onPointerUp;

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
    }
}
