using System.Collections.Generic;
using UnityEngine;

public class QueueBox : MonoBehaviour
{
    public ButtonRequestSpawn buttonRequestSpawn;
    public InputChecker inputChecker;
    public float padding = 0.1f;
    public float scaleSpeed = 6;
    public float secondsFromQueueBoxToCollin = 3;
    public float displayDelay = 1;
    public float secondsAfterTarget = 3;
    public float secondFromBadGuyToQueueBox = 1;

    [SerializeField] private List<Combo> combos = new List<Combo>();
    [SerializeField] private List<GameObject> alreadyAdded = new List<GameObject>();
    private Transform _transform;
    private ScaleTowards _scaleTowards;
    private Coroutine _coroutine;

    private Vector3 Scale => _transform.localScale;
    private float GameTime => buttonRequestSpawn.time;
    private bool HaveCombos => combos.Count > 0;
    private bool IsInCombo => GetCurrentCombo() != null;
    private Vector3 Target => inputChecker.transform.position;
    private Vector3 Target2 => inputChecker.transform.position + Vector3.left * 5;

    private void Awake()
    {
        _transform = transform;
        _scaleTowards = GetComponent<ScaleTowards>();
    }

    private void Update()
    {
        if (IsInCombo)
        {
            var combo = GetCurrentCombo();
            UpdateSize(combo);
        }
        else
            _scaleTowards.SetTarget(Scale.SetX(0), scaleSpeed);

        CreateNewNotes();
        SendQueueToColin();
    }

    private void UpdateSize(Combo combo)
    {
        var start = combo.start - secondsFromQueueBoxToCollin - displayDelay - combo.Duration -
                    secondFromBadGuyToQueueBox;
        var end = combo.end - secondsFromQueueBoxToCollin - displayDelay - combo.Duration;
        var localTime = GameTime - start;
        var timeScale = combo.Duration / (end - start);
        localTime *= timeScale;
        localTime = Mathf.Clamp(localTime, 0, combo.Duration);
        var t = localTime / combo.Duration;
        _scaleTowards.SetTarget(Scale.SetX(t * combo.Width + padding), scaleSpeed);
    }

    private void CreateNewNotes()
    {
        foreach (var combo in combos)
        foreach (var buttonRequest in combo.buttonRequests)
        {
            var createTime = buttonRequest.time;
            createTime -= combo.end - buttonRequest.time;
            createTime -= displayDelay;
            createTime -= secondsFromQueueBoxToCollin;
            createTime -= secondFromBadGuyToQueueBox;
            if (GameTime < createTime) continue;
            if (GameTime >= buttonRequest.time + secondsAfterTarget) continue;
            var buttonRequestGameObject = buttonRequestSpawn.buttonRequestGameObjects[buttonRequest.id];
            buttonRequestGameObject.SetActive(true);
            if (alreadyAdded.Contains(buttonRequestGameObject)) continue;
            inputChecker.buttonRequests.Add(buttonRequestGameObject);
            alreadyAdded.Add(buttonRequestGameObject);
            var moveTowards = buttonRequestGameObject.GetComponent<MoveTowards>();
            moveTowards.SetTarget(transform.position, GameTime + secondFromBadGuyToQueueBox);
        }
    }

    private void SendQueueToColin()
    {
        foreach (var combo in combos)
        foreach (var buttonRequest in combo.buttonRequests)
            if (GameTime >= buttonRequest.time + secondsAfterTarget)
                buttonRequestSpawn.buttonRequestGameObjects[buttonRequest.id].SetActive(false);
            else if (GameTime >= buttonRequest.time)
                buttonRequestSpawn.buttonRequestGameObjects[buttonRequest.id].GetComponent<MoveTowards>()
                    .SetTargetX(Target2.x, buttonRequest.time + secondsAfterTarget);
            else if (GameTime >= buttonRequest.time - secondsFromQueueBoxToCollin)
                buttonRequestSpawn.buttonRequestGameObjects[buttonRequest.id].GetComponent<MoveTowards>()
                    .SetTargetX(Target.x, buttonRequest.time);
            else
            {
                var gameTime = GameTime;
                if (GameTime < buttonRequest.time - secondsFromQueueBoxToCollin - displayDelay)
                    gameTime = buttonRequest.time - secondsFromQueueBoxToCollin - displayDelay;
                var buttonRequestGameObject = buttonRequestSpawn.buttonRequestGameObjects[buttonRequest.id];
                var buttonRequestMoveTowards = buttonRequestGameObject.GetComponent<MoveTowards>();
                var timeAfterStart = buttonRequest.time - combo.start;
                var normalizedTimeAfterStart = timeAfterStart / combo.Duration;
                var comboTime = gameTime - (combo.start - secondsFromQueueBoxToCollin - displayDelay - combo.Duration);
                comboTime = Mathf.Clamp(comboTime, 0, combo.Duration);
                var comboT = comboTime / combo.Duration;
                var boxSize = comboT * combo.Width;
                var boxTarget = transform.position;
                boxTarget += Vector3.right * (normalizedTimeAfterStart * boxSize);
                boxTarget += Vector3.left * boxSize / 2;
                buttonRequestMoveTowards.SetTargetX(boxTarget.x, gameTime);
            }

        // for (var i = combos.Count - 1; i >= 0; i--)
        //     if (GameTime >= combos[i].end + displayDelay - secondsFromQueueBoxToCollin)
        //         combos.RemoveAt(i);
    }

    private Combo GetCurrentCombo()
    {
        if (!HaveCombos) return null;
        foreach (var combo in combos)
        {
            var start = combo.start - secondsFromQueueBoxToCollin - displayDelay - combo.Duration -
                        secondFromBadGuyToQueueBox;
            var end = combo.end - secondsFromQueueBoxToCollin;
            if (start <= GameTime && GameTime <= end)
                return combo;
        }

        return null;
    }

    public void AddCombos(IEnumerable<Combo> newCombos)
    {
        combos.AddRange(newCombos);
    }
}