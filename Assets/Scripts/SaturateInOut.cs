using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaturateInOut : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material material;
    private Coroutine _coroutine;
    [SerializeField] private Vector4 unsaturated = new Vector4(0, -1, -.15f, 0);
    [SerializeField] private float fadeTime = 0.5f;
    private static readonly string Property = "_HSVAAdjust";

    private void OnEnable()
    {
        var image = GetComponent<Image>();
        material = Instantiate(image.material);
        image.material = material;
        material.SetVector(Property, unsaturated);
    }

    private IEnumerator SaturateIn()
    {
        var startingVector = material.GetVector(Property);
        var endingVector = Vector4.zero;
        var time = 0f;
        Debug.Log("SaturateIn");
        while (time < fadeTime)
        {
            var value = Vector4.Lerp(startingVector, endingVector, time / fadeTime);
            material.SetVector(Property, value);
            yield return null;
            time += Time.deltaTime;
        }

        material.SetVector(Property, endingVector);
        Debug.Log(material.GetVector(Property));
        _coroutine = null;
    }

    private IEnumerator SaturateOut()
    {
        var startingVector = material.GetVector(Property);
        var endingVector = unsaturated;
        var time = 0f;
        while (time < fadeTime)
        {
            var value = Vector4.Lerp(startingVector, endingVector, time / fadeTime);
            material.SetVector(Property, value);
            yield return null;
            time += Time.deltaTime;
        }

        material.SetVector(Property, endingVector);
        _coroutine = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(SaturateIn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(SaturateOut());
    }
}