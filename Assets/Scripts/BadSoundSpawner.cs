public class BadSoundSpawner : RandomSoundSpawner
{
    private void OnEnable() => GameEvents.Failed.Add(Spawn);
    private void OnDisable() => GameEvents.Failed?.Remove(Spawn);
}