using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ShowSummaryScreen : MonoBehaviour
{
    private ButtonRequestSpawn _buttonRequestSpawn;
    private QueueBox _queueBox;

    private void Update()
    {
        if (!_buttonRequestSpawn) _buttonRequestSpawn = FindObjectOfType<ButtonRequestSpawn>();
        if (!_queueBox) _queueBox = FindObjectOfType<QueueBox>();
        if (!_buttonRequestSpawn || !_queueBox) return;
        var finishTime = _buttonRequestSpawn.levelData.buttonRequests.Max(x => x.time);
        finishTime += _queueBox.displayDelay;
        finishTime += _queueBox.secondsFromQueueBoxToCollin;
        if (_buttonRequestSpawn.time >= finishTime)
        {
            FindObjectOfType<SummaryPanel>(true).gameObject.SetActive(true);
            FindObjectOfType<HalfFadeOutPanel>(true).gameObject.SetActive(true);
        }
    }
}