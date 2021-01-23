using System.Collections;
using TMPro;
using UnityEngine;

public class IntToTextMeshPro : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public Coroutine coroutine;
    public float resetSeconds = 2;
    public string resetValue = "--";

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetTextFromInt(int value)
    {
        textMesh.text = value.ToString();
    }

    public void SetRedTextFromInt(int value)
    {
        textMesh.text = $"<color=\"red\">{value}</color>";
        StopAndStartCoroutine(SetToValueAfterSeconds());
    }

    public void SetGreenTextFromInt(int value)
    {
        textMesh.text = $"<color=\"green\">{value}</color>";
        StopAndStartCoroutine(SetToValueAfterSeconds());
    }

    private void StopAndStartCoroutine(IEnumerator routine)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(routine);
    }

    private IEnumerator SetToValueAfterSeconds()
    {
        yield return new WaitForSeconds(resetSeconds);
        textMesh.text = resetValue;
        coroutine = null;
    }
}
