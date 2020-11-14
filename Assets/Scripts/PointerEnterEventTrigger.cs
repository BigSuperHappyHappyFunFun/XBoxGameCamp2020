using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerEnterEventTrigger : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent onPointerEnter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }
}