using GoogleSheetsToUnity;
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
        public string button;
        public float start;
        public float end;
        public float speed;
        public string owner;
    }

    public TextAsset level;
    public string sheet = "Sheet1";
    public List<ButtonRequest> buttonRequests;
    public UnityEvent levelLoaded;

    private IEnumerator Start()
    {
        if (level)
        {
            levelLoaded.Invoke();
            LoadLevel(level.text);
        }
        else
        {
            var search = new GSTU_Search("1mr7IMpoF33-i6K8jZh4LotPitlpKIG72MOqd6zweR_E", sheet);
            yield return GoogleAuthrisationHelper.CheckForRefreshOfToken();
            SpreadsheetManager.Read(search, GetLevelText);
        }
    }

    private void GetLevelText(GstuSpreadSheet sheet)
    {
        var levelText = "";
        for (var i = 0; i < sheet.rows.primaryDictionary.Count; i++)
        {
            var key = sheet.rows.primaryDictionary.Keys.ElementAt(i);
            var row = sheet.rows.primaryDictionary[key];
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
                buttonRequest.start = System.Convert.ToSingle(buttonRequestStrings[0]);
                //buttonRequest.end = System.Convert.ToSingle(buttonRequestStrings[1]);
                buttonRequest.button = buttonRequestStrings[2];
                buttonRequest.speed = System.Convert.ToSingle(buttonRequestStrings[3]);
                buttonRequest.name = $"Button {buttonRequest.button} @ {buttonRequest.start}";// - {buttonRequest.end}";
                buttonRequests.Add(buttonRequest);
                if (sheet == "DemoLevel")
                {
                    buttonRequest.owner = buttonRequestStrings[4];
                }
            }
            catch { }
        }
        levelLoaded.Invoke();
        GameEvents.LevelStarted.Invoke();
    }
}
