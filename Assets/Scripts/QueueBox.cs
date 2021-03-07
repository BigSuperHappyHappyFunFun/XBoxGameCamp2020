using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBox : MonoBehaviour
{
    public ButtonRequestSpawn buttonRequestSpawn;
    public InputChecker inputChecker;
    public float padding = 0.1f;
    public float scaleSpeed = 6;
    public ScaleTowards scaleTowards;
    public List<MoveTowards> notes = new List<MoveTowards>();
    public Grouping currentGrouping;
    public float startTime;
    public float scaleFactor = 1;
    public float comboDuration;

    private Vector3 Scale => ((Component) this).transform.localScale;
    private float GameTime => buttonRequestSpawn.time;
    private float DeltaTime => startTime != 0 ? GameTime - startTime : 0;
    private float ClampDeltaTime => Mathf.Min(comboDuration, DeltaTime);
    private new Transform transform;
    private Coroutine coroutine;

    private void OnValidate()
    {
        if (!scaleTowards) scaleTowards = GetComponent<ScaleTowards>();
    }

    private void OnEnable()
    {
        transform = base.transform;
        GameEvents.NewEnemyNote.Add(AddNote);
        GameEvents.NewEnemyNoteGrouping?.Add(AddGrouping);
    }

    private void OnDisable()
    {
        GameEvents.NewEnemyNote?.Remove(AddNote);
        GameEvents.NewEnemyNoteGrouping?.Remove(AddGrouping);
    }

    private void Update()
    {
        if (FinalNoteHasArrived() && coroutine == null)
            coroutine = StartCoroutine(RemoveGrouping());

        if (currentGrouping == null || !currentGrouping.isEnemyGroup) return;
        SetScale();
        SetTargets();
    }

    private bool FinalNoteHasArrived()
    {
        if (currentGrouping == null) return false;
        if (notes.Count <= 0) return false;
        var lastI = notes.Count - 1;
        var lastNote = notes[lastI];
        var lastButtonRequest = currentGrouping.buttonRequests[lastI];
        if (!lastButtonRequest.isComboEnd) return false;
        return Vector3.Distance(lastNote.transform.position, lastNote.target) < 0.01f;
    }

    private void AddGrouping(Grouping grouping)
    {
        currentGrouping = grouping;
        comboDuration = grouping.end - grouping.start;
        startTime = GameTime;
    }

    private void AddNote(GameObject noteGameObject)
    {
        var note = noteGameObject.GetComponent<MoveTowards>();
        if (!note) return;
        var target = ((Component) this).transform.position + (Vector3.right * ClampDeltaTime / 2f);
        var distance = Vector3.Distance(transform.position, noteGameObject.transform.position);
        var speed = distance / buttonRequestSpawn.secondsFromBadGuyToQueueBox;
        note.SetTarget(target, speed);
        notes.Add(note);
    }

    private IEnumerator RemoveGrouping()
    {
        if (currentGrouping != null)
        {
            yield return new WaitForSeconds(buttonRequestSpawn.displayDelay);
            for (var i = 0; i < notes.Count; i++)
            {
                var note = notes[i];
                var buttonRequest = currentGrouping.buttonRequests[i];
                var target = inputChecker.transform.position;
                var distance = Vector3.Distance(note.transform.position, target);
                var deltaTime = buttonRequest.time - currentGrouping.start;
                var time = buttonRequestSpawn.secondsFromQueueBoxToCollin + deltaTime;
                var speed = distance / time;
                target -= Vector3.right * 5;
                note.SetTarget(target, speed);
                inputChecker.buttonRequests.Add(note.gameObject);
                GameEvents.NewPlayerNote.Invoke(note.gameObject);
                Destroy(note.gameObject, time + 1);
            }

            notes.Clear();
            scaleTowards.SetTarget(Scale.SetX(0), scaleSpeed);
            GameEvents.NewPlayerNoteGrouping.Invoke(currentGrouping);
        }

        currentGrouping = null;
        startTime = 0;
        coroutine = null;
    }

    private void SetTargets()
    {
        for (var i = 0; i < notes.Count; i++)
        {
            var secondsFromStart = currentGrouping.buttonRequests[i].time - currentGrouping.start;
            var boxSize = ClampDeltaTime * scaleFactor;
            boxSize -= padding;
            var target = ((Component) this).transform.position;
            target += Vector3.right * secondsFromStart / comboDuration * boxSize;
            target -= Vector3.right * boxSize / 2;
            notes[i].SetTarget(target, notes[i].speed);
        }
    }

    private void SetScale()
    {
        scaleTowards.SetTarget(Scale.SetX(ClampDeltaTime * scaleFactor), scaleSpeed);
    }
}