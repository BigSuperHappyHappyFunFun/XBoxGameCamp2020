using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PointerExitEventTrigger : MonoBehaviour, IPointerExitHandler
{
    public UnityEvent onPointerExit;

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }
}
