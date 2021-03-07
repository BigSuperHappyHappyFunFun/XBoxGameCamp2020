using GoogleSheetsToUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelData : MonoBehaviour
{
    [System.Serializable]
    public class ButtonRequest
    {
        public string name;
        public float time;
        public string button;
        public string combo;
        public bool isComboStart;
        public bool isComboEnd;
    }

    public TextAsset level;
    public string sheet = "Sheet1";
    public List<ButtonRequest> buttonRequests;
    public UnityEvent levelLoaded;
    public ButtonRequestSpawn buttonRequestSpawn;
    
    private IEnumerator Start()
    {
        if (!buttonRequestSpawn) buttonRequestSpawn = FindObjectOfType<ButtonRequestSpawn>(true);
        if (level)
        {
            levelLoaded.Invoke();
            LoadLevel(level.text);
        }
        else
        {
            yield return GoogleAuthrisationHelper.CheckForRefreshOfToken();
            var settingsSearch = new GSTU_Search("1mr7IMpoF33-i6K8jZh4LotPitlpKIG72MOqd6zweR_E", "Settings");
            SpreadsheetManager.Read(settingsSearch, SetSettings);
            
            var levelSearch = new GSTU_Search("1mr7IMpoF33-i6K8jZh4LotPitlpKIG72MOqd6zweR_E", sheet);
            SpreadsheetManager.Read(levelSearch, GetLevelText);
        }
    }

    private void SetSettings(GstuSpreadSheet spreadsheet)
    {
        var csv = "";
        for (var i = 0; i < spreadsheet.rows.primaryDictionary.Count; i++)
        {
            var key = spreadsheet.rows.primaryDictionary.Keys.ElementAt(i);
            var row = spreadsheet.rows.primaryDictionary[key];
            for (var j = 0; j < row.Count; j++)
            {
                var cell = row[j];
                csv += j == 0 ? cell.value : "," + cell.value;
            }
            csv += "\n";
        }

        if (!buttonRequestSpawn) return;
        var lines = csv.Split('\n');
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns[0] == "Seconds from Bad Guy to Queue Box")
                buttonRequestSpawn.secondsFromBadGuyToQueueBox = Convert.ToSingle(columns[1]);
            if (columns[0] == "Queue Box Display Delay")
                buttonRequestSpawn.displayDelay = Convert.ToSingle(columns[1]);
            if (columns[0] == "Seconds from Queue Box to Collin")
                buttonRequestSpawn.secondsFromQueueBoxToCollin = Convert.ToSingle(columns[1]);
        }
    }
    
    private void GetLevelText(GstuSpreadSheet spreadsheet)
    {
        var levelText = "";
        for (var i = 0; i < spreadsheet.rows.primaryDictionary.Count; i++)
        {
            var key = spreadsheet.rows.primaryDictionary.Keys.ElementAt(i);
            var row = spreadsheet.rows.primaryDictionary[key];
            for (var j = 0; j < row.Count; j++)
            {
                var cell = row[j];
                levelText += j == 0 ? cell.value : "," + cell.value;
            }
            levelText += "\n";
        }
        LoadLevel(levelText);
    }

    private void LoadLevel(string levelText)
    {
        var lines = levelText.Split('\n');
        for (var i = 1; i < lines.Length; i++)
        {
            var buttonRequestStrings = lines[i].Split(',');

            if (buttonRequestStrings.Length == 0)
                continue;

            var buttonRequest = new ButtonRequest();
            try
            {
                buttonRequest.time = Convert.ToSingle(buttonRequestStrings[0]);
                buttonRequest.button = buttonRequestStrings[1];
                buttonRequest.name = $"Button {buttonRequest.button} @ {buttonRequest.time}";
                buttonRequests.Add(buttonRequest);
                var combo = buttonRequestStrings.Length >= 3 ? buttonRequestStrings[2] : "";
                buttonRequest.combo = combo;
                buttonRequest.name = $"Button {buttonRequest.button} @ {buttonRequest.time} [{buttonRequest.combo}]";
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"{ex} @ i = {i}");
            }
        }
        AssignComboStart();
        AssignComboEnd();

        levelLoaded.Invoke();
        GameEvents.LevelStarted.Invoke();
    }

    private void AssignComboStart()
    {
        var prevCombo = "";
        var prevOwner = "";
        for (var i = 0; i < buttonRequests.Count; i++)
        {
            if (!string.IsNullOrEmpty(buttonRequests[i].combo))
                if (prevCombo != buttonRequests[i].combo)
                {
                    buttonRequests[i].isComboStart = true;
                    buttonRequests[i].name += "*";
                }
            prevCombo = buttonRequests[i].combo;
        }
    }

    private void AssignComboEnd()
    {
        var prevCombo = "";
        var prevOwner = "";
        for (var i = buttonRequests.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(buttonRequests[i].combo))
                if (prevCombo != buttonRequests[i].combo)
                {
                    buttonRequests[i].isComboEnd = true;
                    buttonRequests[i].name += "!";
                }
            prevCombo = buttonRequests[i].combo;
        }
    }
}
