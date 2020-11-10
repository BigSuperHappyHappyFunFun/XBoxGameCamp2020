using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerDownEventTrigger : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }
}
