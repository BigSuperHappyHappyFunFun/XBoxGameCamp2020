using UnityEngine;

public class AlphaTowards : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float target;
    public float speed;

    private void OnValidate()
    {
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var color = spriteRenderer.material.color;
        color.a = Mathf.Lerp(color.a, target, speed * Time.deltaTime);
        spriteRenderer.material.color = color;
    }

    public void SetTarget(float target, float speed)
    {
        this.enabled = true;
        this.target = target;
        this.speed = speed;
    }
}
