using System;
using System.Collections.Generic;

[Serializable]
public class Grouping
{
    public float start;
    public float end;
    public List<LevelData.ButtonRequest> buttonRequests = new List<LevelData.ButtonRequest>();
    public List<MoveTowards> moveTowards = new List<MoveTowards>();
    public bool isEnemyGroup;

    public override string ToString() => $"Grouping {start} {end} {buttonRequests.Count}";
}