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
    public float secondsFromBadGuyToQueueBox = 0.5f;
    public float secondsFromQueueBoxToCollin = 3;
    public float displayDelay = 1;

    private void Update()
    {
        time += Time.deltaTime;
        if (index >= levelData.buttonRequests.Count)
            return;

        var buttonRequest = levelData.buttonRequests[index];
        if (buttonRequest.isComboStart)
            currentGrouping = GetGrouping();

        var comboDuration = currentGrouping.end - currentGrouping.start;
        var timeToTarget = secondsFromBadGuyToQueueBox;
        timeToTarget += comboDuration;
        timeToTarget += secondsFromQueueBoxToCollin;
        timeToTarget += displayDelay;
        
        var startTime = buttonRequest.time - timeToTarget;
        var timeForNextButton = time >= startTime;
        if (!timeForNextButton) return;
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
        currentGrouping.moveTowards.Add(buttonRequestMoveTowards);

        buttonRequestAnimator.enabled = false;
        buttonRequestGameObject.transform.localScale = Vector3.one;
        if (buttonRequest.isComboStart)
            GameEvents.NewEnemyNoteGrouping.Invoke(currentGrouping);
        GameEvents.NewEnemyNote.Invoke(buttonRequestGameObject);
        index++;
    }

    private Grouping GetGrouping()
    {
        if (currentGrouping != null && currentGrouping.buttonRequests.Count > 0)
            if (currentGrouping.buttonRequests[0] == levelData.buttonRequests[index])
                return currentGrouping;

        var grouping = new Grouping();
        for (var i = index; i < levelData.buttonRequests.Count; i++)
        {
            var buttonRequest = levelData.buttonRequests[i];
            grouping.buttonRequests.Add(buttonRequest);
            if (buttonRequest.isComboStart)
                grouping.isEnemyGroup = true;
            if (buttonRequest.isComboStart)
                grouping.start = buttonRequest.time;
            if (!buttonRequest.isComboEnd) continue;
            grouping.end = buttonRequest.time;
            break;
        }

        return grouping;
    }
}