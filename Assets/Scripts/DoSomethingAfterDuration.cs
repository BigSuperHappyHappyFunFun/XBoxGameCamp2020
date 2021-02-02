using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoSomethingAfterDuration : MonoBehaviour
{
    public float duration = 1;
    public UnityEvent something;
    public Coroutine coroutine;

    private void OnEnable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Something());
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    private IEnumerator Something()
    {
        yield return new WaitForSeconds(duration);
        something.Invoke();
    }
}
