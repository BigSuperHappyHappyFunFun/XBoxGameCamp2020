using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public LevelData levelData;
    public float time = 0;
    private QueueBox _queueBox;

    private void Awake()
    {
        _queueBox = GetComponentInChildren<QueueBox>();
    }

    private void OnEnable()
    {
        _queueBox.AddCombos(CreateCombos());
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private List<Combo> CreateCombos()
    {
        var combos = new List<Combo>();
        var comboLabels = levelData.buttonRequests.Select(x => x.combo).Distinct().OrderBy(x => x);
        foreach (var comboLabel in comboLabels)
        {
            var combo = new Combo();
            combo.buttonRequests =
                new List<LevelData.ButtonRequest>(levelData.buttonRequests.Where(x => x.combo == comboLabel));
            combo.start = combo.buttonRequests.Min(x => x.time);
            combo.end = combo.buttonRequests.Max(x => x.time);
            combos.Add(combo);
        }

        return combos;
    }
}