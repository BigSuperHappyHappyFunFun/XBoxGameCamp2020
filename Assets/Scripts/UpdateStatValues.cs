using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateStatValues : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public InputChecker inputChecker;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = $"{inputChecker.bestCount}\n";
        textMesh.text += $"{inputChecker.betterCount}\n";
        textMesh.text += $"{inputChecker.goodCount}\n";
        textMesh.text += $"{inputChecker.wrongButtonCount}\n";
        textMesh.text += $"{inputChecker.missCount}";
    }
}
