using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    public GameSettings gameSettings;
    public TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
        if (gameSettings && textMesh)
            textMesh.text = $"v{gameSettings.Version}";
    }

    private void OnEnable() => OnValidate();
}
