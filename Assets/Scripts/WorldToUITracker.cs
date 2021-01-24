using UnityEngine;

public class WorldToUITracker : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public RectTransform UI;
    public Vector2 uiPosition;
    public Vector3 worldPosition;

    private void OnValidate()
    {
        if (!camera) camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        uiPosition = UI.position;
        uiPosition.y -= UI.rect.height / 2;
        worldPosition = camera.ScreenToWorldPoint(uiPosition);
        worldPosition.z = 0;
        transform.position = worldPosition;
    }
}
