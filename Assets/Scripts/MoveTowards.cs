using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Vector3 target;
    public float speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void SetTarget(Vector3 target, float speed)
    {
        this.enabled = true;
        this.target = target;
        this.speed = speed;
    }
}
