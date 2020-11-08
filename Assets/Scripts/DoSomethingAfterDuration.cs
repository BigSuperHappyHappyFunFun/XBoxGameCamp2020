using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAfterDuration : MonoBehaviour
{
    public float duration = 1;
    public UnityEvent something;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        something.Invoke();
    }
}
