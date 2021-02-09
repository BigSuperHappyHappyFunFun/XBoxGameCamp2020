using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueBox : MonoBehaviour
{
    public float speed = 6;
    public float spacing = 0.1f;
    public ScaleTowards scaleTowards;
    public Vector3[] targets = new Vector3[0];
    public List<MoveTowards> notes = new List<MoveTowards>();

    private void OnValidate()
    {
        if (!scaleTowards) scaleTowards = GetComponent<ScaleTowards>();
    }

    private void OnEnable()
    {
        GameEvents.NewEnemyNote.Add(AddNote);
        GameEvents.NewPlayerNote.Add(RemoveNote);
    }

    private void OnDisable()
    {
        GameEvents.NewEnemyNote?.Remove(AddNote);
        GameEvents.NewPlayerNote?.Remove(RemoveNote);
    }

    public void AddNote(GameObject noteGameObject)
    {
        var note = noteGameObject.GetComponent<MoveTowards>();
        if (note)
        {
            notes.Add(note);
            SetTargets();
            SetScale();
        }
    }

    public void RemoveNote(GameObject noteGameObject)
    {
        var note = noteGameObject.GetComponent<MoveTowards>();
        if (note)
        {
            Destroy(notes[0].gameObject);
            notes.RemoveAt(0);
            SetTargets();
            SetScale();
        }
    }

    private void SetTargets()
    {
        var count = notes.Count;
        if (count == 0)
            targets = new Vector3[0];
        else
        {
            targets = new Vector3[count];
            var width = (1 + spacing) * count - spacing;
            for (var i = 0; i < count; i++)
            {
                var myPosition = transform.position;
                var offset = -width / 2 + 0.5f;
                var positionX = (1 + spacing) * i;
                positionX += offset;
                positionX += myPosition.x;
                targets[i] = new Vector3(positionX, myPosition.y, myPosition.z);
            }
        }

        for (var i = 0; i < count; i++)
            notes[i].SetTarget(targets[i], speed);
    }

    private void SetScale()
    {
        var count = notes.Count;
        var width = (1.25f + spacing) * count - spacing;
        var scale = transform.localScale;

        if (count == 0)
            scaleTowards.SetTarget(new Vector3(0, scale.y, scale.z), speed);
        else
            scaleTowards.SetTarget(new Vector3(width, scale.y, scale.z), speed);
    }
}
