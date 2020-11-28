public class GoodSoundSpawner : RandomSoundSpawner
{
    private void OnEnable()
    {
        GameEvents.PressedCorrect.Add(Spawn);
    }

    private void OnDisable()
    {
        GameEvents.PressedCorrect?.Remove(Spawn);
    }
}