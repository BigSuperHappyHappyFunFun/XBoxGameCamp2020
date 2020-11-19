using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    public UnityEvent onEnabled;

    private void OnEnable()
    {
        onEnabled.Invoke();
    }
}
