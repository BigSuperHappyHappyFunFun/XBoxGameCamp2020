using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRequestMove : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
