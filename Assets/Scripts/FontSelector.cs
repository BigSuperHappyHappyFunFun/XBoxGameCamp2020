using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontSelector : MonoBehaviour
{
    public static FontSelector instance;

    public int index;
    public List<TMP_FontAsset> fontAssets = new List<TMP_FontAsset>();
    public Coroutine coroutine;
    public TextMeshProUGUI fontNameTextMesh;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(UpdateFontEverySecond());
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    private IEnumerator UpdateFontEverySecond()
    {
        while (enabled)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            if (fontAssets.Count > 0)
                foreach (var textMesh in FindObjectsOfType<TextMeshProUGUI>())
                    textMesh.font = fontAssets[index];
        }
    }

    private void Update()
    {
        fontNameTextMesh.text = fontAssets[index].name;
        if (Input.GetKeyDown(KeyCode.F))
            index = (index + 1) % fontAssets.Count;
    }
}
