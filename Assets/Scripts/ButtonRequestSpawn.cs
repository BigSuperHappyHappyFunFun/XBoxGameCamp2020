using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public Transform spawn;
    public Transform target;
    public LevelData levelData;
    public GameObject buttonRequestPrefab;
    public InputChecker inputChecker;
    public int index = 0;
    public float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (index >= levelData.buttonRequests.Count)
            return;

        var animationLength = 5;
        var buttonRequest = levelData.buttonRequests[index];
        var timeToTarget = animationLength / buttonRequest.speed;
        var startTime = buttonRequest.start - timeToTarget;
        var timeForNextButton = time >= startTime;
        if (timeForNextButton)
        {
            var buttonRequestGameObject = Instantiate(buttonRequestPrefab, spawn.position, Quaternion.identity, transform);
            var buttonRequestCombo = buttonRequestGameObject.GetComponent<ButtonRequestCombo>();
            var buttonRequestAnimator = buttonRequestGameObject.GetComponent<Animator>();
            var buttonRequestImage = buttonRequestGameObject.GetComponent<ButtonRequestImage>();
            buttonRequestImage.ShowArrow(buttonRequest.button);
            buttonRequestCombo.combo = buttonRequest.combo;
            buttonRequestCombo.isStart = buttonRequest.isComboStart;
            buttonRequestCombo.isEnd = buttonRequest.isComboEnd;
            buttonRequestGameObject.name = buttonRequest.button;
            buttonRequestAnimator.speed = buttonRequest.speed;

            if (buttonRequest.owner != "Enemy")
            {
                inputChecker.buttonRequests.Add(buttonRequestGameObject);
                GameEvents.NewPlayerNote.Invoke(buttonRequestGameObject);
            }
            else
            {
                buttonRequestAnimator.enabled = false;
                buttonRequestGameObject.transform.localScale = Vector3.one;
                GameEvents.NewEnemyNote.Invoke(buttonRequestGameObject);
            }

            index++;
        }
    }
}
