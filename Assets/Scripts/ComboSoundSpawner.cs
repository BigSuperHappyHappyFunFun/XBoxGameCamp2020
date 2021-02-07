public class ComboSoundSpawner : RandomSoundSpawner
{
    private void OnEnable()
    {
        GameEvents.FinishedCombo.Add(Spawn);
    }

    private void OnDisable()
    {
        GameEvents.FinishedCombo?.Remove(Spawn);
    }

    private void Spawn(int i) => Spawn();
}