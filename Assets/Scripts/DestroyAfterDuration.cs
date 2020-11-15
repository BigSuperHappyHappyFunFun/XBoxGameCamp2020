using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour
{
    public float duration = 1;
    public bool destroyGameObject = true;

    private void Start()
    {
        if (destroyGameObject)
            Destroy(gameObject, duration);
        else
            Destroy(this, duration);
    }
}
