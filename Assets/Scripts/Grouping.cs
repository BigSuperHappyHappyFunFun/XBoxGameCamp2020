using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Grouping
{
    public float start;
    public float end;
    public List<LevelData.ButtonRequest> list = new List<LevelData.ButtonRequest>();
    public List<MoveTowards> moveTowards = new List<MoveTowards>();

    public override string ToString() => $"Grouping {start} {end} {list.Count}";
}