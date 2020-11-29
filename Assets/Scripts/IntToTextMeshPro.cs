using TMPro;
using UnityEngine;

public class IntToTextMeshPro : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

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
    }

    public void SetGreenTextFromInt(int value)
    {
        textMesh.text = $"<color=\"green\">{value}</color>";
    }
}
