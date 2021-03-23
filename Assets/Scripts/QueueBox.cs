using System.Collections.Generic;
using UnityEngine;

public class QueueBox : MonoBehaviour
{
    public ButtonRequestSpawn buttonRequestSpawn;
    public InputChecker inputChecker;
    public float padding = 0.1f;
    public float scaleSpeed = 6;
    public GameObject buttonRequestPrefab;
    public float secondsFromQueueBoxToCollin = 3;
    public float displayDelay = 1;

    [SerializeField] private List<Combo> combos = new List<Combo>();
    private Transform _transform;
    private ScaleTowards _scaleTowards;
    private Coroutine _coroutine;

    private Dictionary<LevelData.ButtonRequest, MoveTowards> _moveTargets =
        new Dictionary<LevelData.ButtonRequest, MoveTowards>();

    private Vector3 Scale => _transform.localScale;
    private float GameTime => buttonRequestSpawn.time;
    private bool HaveCombos => combos.Count > 0;
    private bool IsInCombo => GetCurrentCombo() != null;

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
            CreateNewNotes(combo);
            MoveNotes(combo);
        }

        SendQueueToColin();
    }

    private void UpdateSize(Combo combo)
    {
        var localTime = GameTime - combo.start + combo.Duration + displayDelay + secondsFromQueueBoxToCollin;
        localTime = Mathf.Clamp(localTime, 0, combo.Duration);
        var t = localTime / combo.Duration;
        _scaleTowards.SetTarget(Scale.SetX(t * combo.Width), scaleSpeed);
    }

    private void CreateNewNotes(Combo combo)
    {
        foreach (var buttonRequest in combo.buttonRequests)
        {
            var createTime = buttonRequest.time;
            createTime -= combo.end - buttonRequest.time;
            createTime -= displayDelay;
            createTime -= secondsFromQueueBoxToCollin;
            if (GameTime >= createTime)
                if (!_moveTargets.ContainsKey(buttonRequest))
                {
                    var spawnPosition = _transform.position + Vector3.down * 5;
                    var buttonRequestGameObject = Instantiate(buttonRequestPrefab, spawnPosition, Quaternion.identity,
                        buttonRequestSpawn.transform);
                    var buttonRequestCombo = buttonRequestGameObject.GetComponent<ButtonRequestCombo>();
                    var buttonRequestAnimator = buttonRequestGameObject.GetComponent<Animator>();
                    var buttonRequestImage = buttonRequestGameObject.GetComponent<ButtonRequestImage>();
                    var buttonRequestMoveTowards = buttonRequestGameObject.GetComponent<MoveTowards>();
                    buttonRequestImage.ShowArrow(buttonRequest.button);
                    buttonRequestCombo.combo = buttonRequest.combo;
                    buttonRequestCombo.isStart = buttonRequest.isComboStart;
                    buttonRequestCombo.isEnd = buttonRequest.isComboEnd;
                    buttonRequestGameObject.name = buttonRequest.button;
                    buttonRequestAnimator.enabled = false;
                    buttonRequestGameObject.transform.localScale = Vector3.one;
                    _moveTargets.Add(buttonRequest, buttonRequestMoveTowards);
                    inputChecker.buttonRequests.Add(buttonRequestGameObject);
                }
        }
    }

    private void SendQueueToColin()
    {
        foreach (var combo in combos)
            if (GameTime >= combo.end + displayDelay)
                foreach (var buttonRequest in combo.buttonRequests)
                {
                    SetFinalTarget(_moveTargets[buttonRequest], combo, buttonRequest);
                }

        for (var i = combos.Count - 1; i >= 0; i--)
            if (GameTime >= combos[i].end + displayDelay)
                combos.RemoveAt(i);
    }

    private Combo GetCurrentCombo()
    {
        if (!HaveCombos) return null;
        foreach (var combo in combos)
        {
            var start = combo.start - combo.Duration - displayDelay - secondsFromQueueBoxToCollin;
            var end = combo.end + displayDelay;
            if (start <= GameTime && GameTime <= end)
                return combo;
        }

        return null;
    }

    private void SetFinalTarget(MoveTowards moveTarget, Combo combo, LevelData.ButtonRequest buttonRequest)
    {
        var localTime = buttonRequest.time - combo.start;
        var t = localTime / combo.Duration;
        var target = inputChecker.transform.position;
        var finalTarget = target;
        finalTarget += Vector3.left * 5;
        finalTarget += Vector3.left * combo.Width;
        finalTarget += Vector3.right * (t * combo.Width);
        target += Vector3.right * (t * combo.Width);
        var distance = moveTarget.transform.position.x - target.x;
        moveTarget.SetTarget(finalTarget, distance/secondsFromQueueBoxToCollin);
    }
   
    private void MoveNotes(Combo combo)
    {
        foreach (var buttonRequest in combo.buttonRequests)
        {
            if (!_moveTargets.ContainsKey(buttonRequest)) continue;
            var buttonTime = buttonRequest.time - combo.start;
            var buttonT = buttonTime / combo.Duration;
            var comboTime = GameTime - combo.start;
            comboTime = Mathf.Clamp(comboTime, 0, combo.Duration);
            var comboT = comboTime / combo.Duration;
            var boxSize = comboT * combo.Width;
            boxSize -= padding;
            var boxTarget = transform.position;
            boxTarget += Vector3.right * (buttonT * boxSize);
            boxTarget -= Vector3.right * boxSize / 2;
            _moveTargets[buttonRequest].SetTarget(boxTarget, 10);
        }
    }

    public void AddCombos(IEnumerable<Combo> newCombos)
    {
        combos.AddRange(newCombos);
    }
}