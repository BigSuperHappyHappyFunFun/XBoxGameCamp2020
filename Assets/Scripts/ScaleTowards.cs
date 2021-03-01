using UnityEngine;

public class ScaleTowards : MonoBehaviour
{
    public Vector3 target;
    public float speed;

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, target, speed * Time.deltaTime);
    }

    public void SetTarget(Vector3 target) => SetTarget(target, speed);

    public void SetTarget(Vector3 target, float speed)
    {
        this.enabled = true;
        this.target = target;
        this.speed = speed;
    }
}