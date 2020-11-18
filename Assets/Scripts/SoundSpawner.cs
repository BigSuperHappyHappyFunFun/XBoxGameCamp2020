using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
    public GameObject soundPrefab;

    public void Spawn()
    {
        Instantiate(soundPrefab, transform);
    }
}
