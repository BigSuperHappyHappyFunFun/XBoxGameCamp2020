using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicNoteSpawner : MonoBehaviour
{
    public GameObject musicNotePrefab;

    public void Spawn()
    {
        var musicNote = Instantiate(musicNotePrefab);
        musicNote.transform.SetParent(transform);
        musicNote.transform.localPosition = musicNotePrefab.transform.position;
        musicNote.transform.localRotation = musicNotePrefab.transform.rotation;
        musicNote.transform.localScale = musicNotePrefab.transform.lossyScale;
    }
}
