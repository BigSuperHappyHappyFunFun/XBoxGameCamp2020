using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public Transform spawn;
    public Transform target;
    public LevelData levelData;
    public GameObject buttonRequestPrefab;
    public InputChecker inputChecker;
    public Grouping currentGrouping;
    public int index = 0;
    public float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (index >= levelData.buttonRequests.Count)
            return;

        var secondsFromQueueBoxToCollin = 5;
        var buttonRequest = levelData.buttonRequests[index];
        var timeToTarget = secondsFromQueueBoxToCollin / buttonRequest.speed;
        var startTime = buttonRequest.start - timeToTarget;
        var timeForNextButton = time >= startTime;
        if (timeForNextButton)
        {
            var buttonRequestGameObject = Instantiate(buttonRequestPrefab, spawn.position, Quaternion.identity, transform);
            var buttonRequestCombo = buttonRequestGameObject.GetComponent<ButtonRequestCombo>();
            var buttonRequestAnimator = buttonRequestGameObject.GetComponent<Animator>();
            var buttonRequestImage = buttonRequestGameObject.GetComponent<ButtonRequestImage>();
            var buttonRequestMoveTowards = buttonRequestGameObject.GetComponent<MoveTowards>();
            buttonRequestImage.ShowArrow(buttonRequest.button);
            buttonRequestCombo.combo = buttonRequest.combo;
            buttonRequestCombo.isStart = buttonRequest.isComboStart;
            buttonRequestCombo.isEnd = buttonRequest.isComboEnd;
            buttonRequestGameObject.name = buttonRequest.button;
            buttonRequestAnimator.speed = buttonRequest.speed;
            
            if (buttonRequest.isComboStart)
                currentGrouping = GetGrouping();

            currentGrouping.moveTowards.Add(buttonRequestMoveTowards);

            if (buttonRequest.owner != "Enemy")
            {
                inputChecker.buttonRequests.Add(buttonRequestGameObject);
                GameEvents.NewPlayerNote.Invoke(buttonRequestGameObject);
                if (buttonRequest.isComboStart)
                    GameEvents.NewPlayerNoteGrouping.Invoke(currentGrouping);
            }
            else
            {
                buttonRequestAnimator.enabled = false;
                buttonRequestGameObject.transform.localScale = Vector3.one;
                GameEvents.NewEnemyNote.Invoke(buttonRequestGameObject);
                if (buttonRequest.isComboStart)
                    GameEvents.NewEnemyNoteGrouping.Invoke(currentGrouping);
            }

            index++;
        }
    }

    private Grouping GetGrouping()
    {
        var grouping = new Grouping();
        for (var i = index; i < levelData.buttonRequests.Count; i++)
        {
            var buttonRequest = levelData.buttonRequests[i];
            grouping.list.Add(buttonRequest);
            if (buttonRequest.isComboStart)
                grouping.start = buttonRequest.start;
            if (buttonRequest.isComboEnd)
            {
                grouping.end = buttonRequest.start;
                break;
            }
        }
        return grouping;
    }
}
