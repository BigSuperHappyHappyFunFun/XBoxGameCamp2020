using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Vector3 target;
    public float time;
    public List<Target> targets = new List<Target>();
    private ButtonRequestSpawn _buttonRequestSpawn;

    public float GameTime => _buttonRequestSpawn ? _buttonRequestSpawn.time : Time.time;

    private void Awake()
    {
        _buttonRequestSpawn = FindObjectOfType<ButtonRequestSpawn>();
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            target = targets[0].target;
            time = targets[0].time;
            var myPosition = transform.position;
            var distance = Vector3.Distance(myPosition, target);
            var deltaTime = Mathf.Clamp(time - GameTime, 0, 10000);
            var speed = distance / deltaTime;
            transform.position = Vector3.MoveTowards(myPosition, target, speed * Time.deltaTime);
            if (myPosition == target)
                targets.RemoveAt(0);
        }
    }

    public void AddTarget(Vector3 target, float time)
    {
        targets.Add(new Target {target = target, time = time});
    }
    
    public void SetTarget(Vector3 target, float time)
    {
        targets.Clear();
        targets.Add(new Target {target = target, time = time});
    }

    [Serializable]
    public class Target
    {
        public Vector3 target;
        public float time;
    }
}