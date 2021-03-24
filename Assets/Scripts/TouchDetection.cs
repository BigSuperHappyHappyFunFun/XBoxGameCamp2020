using UnityEngine;

public class TouchDetection : MonoBehaviour
{
    public GameSettings gameSettings;
    public float lastTouch = int.MinValue;
    public float touchTimeout = 60;
    
    private bool TouchDetected => lastTouch >= Time.time - touchTimeout;

    private void Update()
    {
        if (Input.touchCount > 0)
            lastTouch = Time.time;
        
        gameSettings.detectTouchControls = TouchDetected;
    }
}
