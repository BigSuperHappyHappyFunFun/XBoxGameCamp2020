using System.Collections.Generic;
using UnityEngine;

public class RandomSoundSpawner : MonoBehaviour
{
    public List<GameObject> soundPrefabs;

    public void Spawn()
    {
        var randomI = Random.Range(0, soundPrefabs.Count);
        Instantiate(soundPrefabs[randomI], transform);
    }
}
