using System.Collections;
using UnityEngine;

public class MusicNoteSpawner : MonoBehaviour
{
    public GameObject musicNotePrefab;

    private void OnEnable()
    {
        GameEvents.PressedCorrect.Add(Spawn);
    }

    private void OnDisable()
    {
        GameEvents.PressedCorrect?.Remove(Spawn);
    }

    public void Spawn()
    {
        var musicNote = Instantiate(musicNotePrefab);
        musicNote.transform.SetParent(transform);
        musicNote.transform.localPosition = musicNotePrefab.transform.position;
        musicNote.transform.localRotation = musicNotePrefab.transform.rotation;
        musicNote.transform.localScale = musicNotePrefab.transform.lossyScale;
    }
}
