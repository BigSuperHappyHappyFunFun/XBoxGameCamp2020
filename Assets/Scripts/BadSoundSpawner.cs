public class BadSoundSpawner : SoundSpawner
{
    private void OnEnable() => GameEvents.Failed.Add(Spawn);
    private void OnDisable() => GameEvents.Failed?.Remove(Spawn);
}