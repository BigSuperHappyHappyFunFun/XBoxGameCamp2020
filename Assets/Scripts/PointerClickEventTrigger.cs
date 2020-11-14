using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerClickEventTrigger : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onPointerClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick.Invoke();
    }
}