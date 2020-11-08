using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRequestDelete : MonoBehaviour
{
    public Vector3 spawnPosition = Vector3.zero;
    public float deleteDistance = 24;

    void Update()
    {
        if (Vector3.Distance(transform.position, spawnPosition) >= deleteDistance)
            Destroy(gameObject);
    }
}
