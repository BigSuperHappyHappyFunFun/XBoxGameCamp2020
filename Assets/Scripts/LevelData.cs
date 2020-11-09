using System.Collections;
using System.Collections.Generic;
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
    }

    public TextAsset level;
    public List<ButtonRequest> buttonRequests;
    public UnityEvent levelLoaded;

    private IEnumerator Start()
    {
        var levelText = "";
        if (level)
        {
            levelText = level.text;
        }
        else
        {
            using(var www = new WWW("https://docs.google.com/spreadsheets/d/e/2PACX-1vRUFB4LsAxD9P7q6MeiWFO7PNKL5EoH817Tf8ouyCkUUZ5lQI2K4F8zgcPDJzCpFlmlJS5rrh_T8U2t/pub?output=csv"))
            {
                yield return www;
                levelText = www.text;
            }
        }

        levelLoaded.Invoke();

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
                buttonRequest.end = System.Convert.ToSingle(buttonRequestStrings[1]);
                buttonRequest.button = buttonRequestStrings[2];
                buttonRequest.speed = System.Convert.ToSingle(buttonRequestStrings[3]);
                buttonRequest.name = $"Button {buttonRequest.button} @ {buttonRequest.start} - {buttonRequest.end}";
                buttonRequests.Add(buttonRequest);
            }
            catch { }
        }
    }
}
