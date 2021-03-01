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
    public ScaleTowards scaleTowards;
    public List<MoveTowards> notes = new List<MoveTowards>();
    public Grouping currentGrouping;
    public float startTime;
    public float scaleFactor = 1;
    public float comboDuration = 0;

    private Vector3 Scale => transform.localScale;
    public float GameTime => buttonRequestSpawn.time;
    public float DeltaTime => startTime != 0 ? GameTime - startTime : 0;
    public float ClampDeltaTime => Mathf.Min(comboDuration, DeltaTime);

    private void OnValidate()
    {
        if (!scaleTowards) scaleTowards = GetComponent<ScaleTowards>();
    }

    private void OnEnable()
    {
        GameEvents.NewEnemyNote.Add(AddNote);
        GameEvents.NewEnemyNoteGrouping?.Add(AddGrouping);
        GameEvents.NewPlayerNoteGrouping?.Add(RemoveGrouping);
    }

    private void OnDisable()
    {
        GameEvents.NewEnemyNote?.Remove(AddNote);
        GameEvents.NewEnemyNoteGrouping?.Remove(AddGrouping);
        GameEvents.NewPlayerNoteGrouping?.Remove(RemoveGrouping);
    }

    private void Update()
    {
        if (currentGrouping != null && currentGrouping.isEnemyGroup)
        {
            SetScale();
            SetTargets();
        }
    }

    public void AddGrouping(Grouping grouping)
    {
        currentGrouping = grouping;
        comboDuration = grouping.end - grouping.start;
        startTime = GameTime;
    }

    public void AddNote(GameObject noteGameObject)
    {
        var note = noteGameObject.GetComponent<MoveTowards>();
        if (note)
        {
            var target = transform.position + (Vector3.right * ClampDeltaTime / 2f);
            var distance = Vector3.Distance(transform.position, noteGameObject.transform.position);
            var speed = distance / buttonRequestSpawn.secondsFromBadGuyToQueueBox;
            note.SetTarget(target, speed);
            notes.Add(note);
        }
    }

    public void RemoveGrouping(Grouping grouping)
    {
        if (currentGrouping != null)
        {
            for (var i = 0; i < notes.Count; i++)
            {
                var note = notes[i];
                var buttonRequest = currentGrouping.buttonRequests[i];
                var target = inputChecker.transform.position;
                var distance = Vector3.Distance(note.transform.position, target);
                var deltaTime = buttonRequest.start - currentGrouping.start;
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
        }
        currentGrouping = grouping;
        startTime = 0;
    }

    private void SetTargets()
    {
        for (var i = 0; i < notes.Count; i++)
        {
            var secondsFromStart = currentGrouping.buttonRequests[i].start - currentGrouping.start;
            var boxSize = ClampDeltaTime * scaleFactor;
            boxSize -= padding;
            var target = transform.position;
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
