using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBox : MonoBehaviour
{
    public ButtonRequestSpawn buttonRequestSpawn;
    public InputChecker inputChecker;
    public float padding = 0.1f;
    public float scaleSpeed = 6;
    public List<MoveTowards> notes = new List<MoveTowards>();
    public Combo currentCombo;
    public float startTime;
    public float scaleFactor = 1;
    public float comboDuration;
    public GameObject buttonRequestPrefab;
    public float secondsFromBadGuyToQueueBox = 0.5f;
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
    private float DeltaTime => startTime != 0 ? GameTime - startTime : 0;
    private float ClampDeltaTime => Mathf.Min(comboDuration, DeltaTime);
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
        var localTime = GameTime - combo.start;
        localTime = Mathf.Clamp(localTime, 0, combo.Duration);
        var t = localTime / combo.Duration;
        _scaleTowards.SetTarget(Scale.SetX(t * combo.Width), scaleSpeed);
    }

    private void CreateNewNotes(Combo combo)
    {
        foreach (var buttonRequest in combo.buttonRequests)
            if (GameTime >= buttonRequest.time)
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
                }
    }

    private void SendQueueToColin()
    {
        foreach (var combo in combos)
            if (GameTime >= combo.end + displayDelay)
                foreach (var buttonRequest in combo.buttonRequests)
                    SetFinalTarget(_moveTargets[buttonRequest], combo, buttonRequest);

        for (var i = combos.Count - 1; i >= 0; i--)
            if (GameTime >= combos[i].end + displayDelay)
                combos.RemoveAt(i);
    }

    private Combo GetCurrentCombo()
    {
        if (!HaveCombos) return null;
        foreach (var combo in combos)
            if (combo.start <= GameTime && GameTime <= combo.end + displayDelay)
                return combo;
        return null;
    }

    private void SetFinalTarget(MoveTowards moveTarget, Combo combo, LevelData.ButtonRequest buttonRequest)
    {
        var localTime = buttonRequest.time - combo.start;
        var t = localTime / combo.Duration;
        var finalTarget = inputChecker.transform.position;
        finalTarget += Vector3.left * 5;
        finalTarget += Vector3.left * combo.Width;
        finalTarget += Vector3.right * (t * combo.Width);
        moveTarget.SetTarget(finalTarget, 5);
    }
    //
    // private bool FinalNoteHasArrived()
    // {
    //     if (currentCombo == null) return false;
    //     if (notes.Count <= 0) return false;
    //     var lastI = notes.Count - 1;
    //     var lastNote = notes[lastI];
    //     var lastButtonRequest = currentCombo.buttonRequests[lastI];
    //     if (!lastButtonRequest.isComboEnd) return false;
    //     return Vector3.Distance(lastNote.transform.position, lastNote.target) < 0.01f;
    // }
    //
    // private void AddGrouping(Combo combo)
    // {
    //     currentCombo = combo;
    //     comboDuration = combo.end - combo.start;
    //     startTime = GameTime;
    // }
    //
    // private void AddNote(GameObject noteGameObject)
    // {
    //     var note = noteGameObject.GetComponent<MoveTowards>();
    //     if (!note) return;
    //     var target = ((Component) this).transform.position + (Vector3.right * ClampDeltaTime / 2f);
    //     var distance = Vector3.Distance(_transform.position, noteGameObject.transform.position);
    //     var speed = distance / buttonRequestSpawn.secondsFromBadGuyToQueueBox;
    //     note.SetTarget(target, speed);
    //     notes.Add(note);
    // }
    //
    // private IEnumerator RemoveGrouping()
    // {
    //     if (currentCombo != null)
    //     {
    //         yield return new WaitForSeconds(buttonRequestSpawn.displayDelay);
    //         for (var i = 0; i < notes.Count; i++)
    //         {
    //             var note = notes[i];
    //             var buttonRequest = currentCombo.buttonRequests[i];
    //             var target = inputChecker.transform.position;
    //             var distance = Vector3.Distance(note.transform.position, target);
    //             var deltaTime = buttonRequest.time - currentCombo.start;
    //             var time = buttonRequestSpawn.secondsFromQueueBoxToCollin + deltaTime;
    //             var speed = distance / time;
    //             target -= Vector3.right * 5;
    //             note.SetTarget(target, speed);
    //             inputChecker.buttonRequests.Add(note.gameObject);
    //             GameEvents.NewPlayerNote.Invoke(note.gameObject);
    //             Destroy(note.gameObject, time + 1);
    //         }
    //
    //         notes.Clear();
    //         _scaleTowards.SetTarget(Scale.SetX(0), scaleSpeed);
    //         GameEvents.NewPlayerNoteGrouping.Invoke(currentCombo);
    //     }
    //
    //     currentCombo = null;
    //     startTime = 0;
    //     _coroutine = null;
    // }

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