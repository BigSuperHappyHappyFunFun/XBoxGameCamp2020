using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicCameraSize : MonoBehaviour
{
    const float SIXTEEN_NINE = 16f / 9f;

    public enum ResizeMethod { Viewport, OrthographicSize }

    public new Camera camera;
    public ResizeMethod resizeMethod = ResizeMethod.Viewport;
    public float cameraOrthographicSize = 6;

    private Rect viewport = new Rect(0, 0, 1, 1);

    private void OnValidate()
    {
        if (!camera) camera = GetComponent<Camera>();
    }

    private void Update()
    {
        var ratio = (float)Screen.width / Screen.height / SIXTEEN_NINE;
        if (resizeMethod == ResizeMethod.Viewport)
        {
            viewport.height = ratio;
            viewport.y = (1 - viewport.height) / 2;
            camera.rect = viewport;
            camera.orthographicSize = cameraOrthographicSize;
        }
        else
        {
            viewport.height = 1;
            viewport.y = 0;
            camera.rect = viewport;
            camera.orthographicSize = cameraOrthographicSize / ratio;
        }
    }
}
