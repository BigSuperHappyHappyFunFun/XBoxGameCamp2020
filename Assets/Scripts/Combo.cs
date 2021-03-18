using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Combo
{
    public float start;
    public float end;
    public List<LevelData.ButtonRequest> buttonRequests = new List<LevelData.ButtonRequest>();

    public float Duration => end - start;
    public float ScaleFactor => 6;
    public float MaxWidth => 10;
    public float Width => Mathf.Min(MaxWidth, Duration * ScaleFactor);

    public override string ToString() => $"Grouping {start} {end} {buttonRequests.Count}";
}