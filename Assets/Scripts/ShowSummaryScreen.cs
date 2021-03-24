using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ShowSummaryScreen : MonoBehaviour
{
    private ButtonRequestSpawn _buttonRequestSpawn;
    private QueueBox _queueBox;
    private bool _summaryScreenShown;

    private void Update()
    {
        if (!_buttonRequestSpawn) _buttonRequestSpawn = FindObjectOfType<ButtonRequestSpawn>();
        if (!_queueBox) _queueBox = FindObjectOfType<QueueBox>();
        if (!_buttonRequestSpawn || !_queueBox) return;
        var finishTime = _buttonRequestSpawn.levelData.buttonRequests.Max(x => x.time);
        finishTime += _queueBox.secondsAfterTarget;
        if (_buttonRequestSpawn.time >= finishTime && !_summaryScreenShown)
        {
            _summaryScreenShown = true;
            var summaryPanel = FindObjectOfType<SummaryPanel>(true);
            var halfFadeOutPanel = FindObjectOfType<HalfFadeOutPanel>(true);
            if (summaryPanel) summaryPanel.gameObject.SetActive(true);
            if (halfFadeOutPanel) halfFadeOutPanel.gameObject.SetActive(true);
        }
    }
}