using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Vector3 target;
    public float time;
    private ButtonRequestSpawn _buttonRequestSpawn;
    public float timeX = -1;
    public float posX;

    public float GameTime => _buttonRequestSpawn ? _buttonRequestSpawn.time : Time.time;

    private void Awake()
    {
        _buttonRequestSpawn = FindObjectOfType<ButtonRequestSpawn>();
    }

    private void Update()
    {
        var myPosition = UpdateTargetYZ();
        UpdateTargetX(myPosition);
    }

    private Vector3 UpdateTargetYZ()
    {
        var myPosition = transform.position;
        var targetYZ = target;
        targetYZ.x = myPosition.x;
        var distance = Vector3.Distance(myPosition, targetYZ);
        var deltaTime = Mathf.Clamp(time - GameTime, 0, 10000);
        if (deltaTime == 0) return myPosition;
        var speed = distance / deltaTime;
        myPosition = Vector3.MoveTowards(myPosition, targetYZ, speed * Time.deltaTime);
        transform.position = myPosition;
        return myPosition;
    }

    private void UpdateTargetX(Vector3 myPosition)
    {
        var targetX = myPosition;
        targetX.x = posX;
        var distanceX = Mathf.Abs(targetX.x - myPosition.x);
        var deltaTimeX = Mathf.Clamp(timeX - GameTime, 0, 10000);
        if (deltaTimeX == 0) return;
        var speedX = distanceX / deltaTimeX;
        transform.position = Vector3.MoveTowards(myPosition, targetX, speedX * Time.deltaTime);
    }

    public void SetTarget(Vector3 target, float time)
    {
        this.target = target;
        this.time = time;
        timeX = time;
        posX = target.x;
    }

    public void SetTargetX(float posX, float timeX)
    {
        this.timeX = timeX;
        this.posX = posX;
    }
}