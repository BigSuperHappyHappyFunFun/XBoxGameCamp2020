using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        var lines = level.text.Split('\n');
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
