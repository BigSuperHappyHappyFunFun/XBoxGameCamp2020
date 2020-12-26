using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public Transform spawn;
    public Transform target;
    public LevelData levelData;
    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    public Dictionary<string, Color> colors = new Dictionary<string, Color>();
    public List<Sprite> spriteList = new List<Sprite>();
    public List<Color> colorList = new List<Color>();
    public GameObject buttonRequestPrefab;
    public InputChecker inputChecker;
    public int index = 0;
    public float time = 0;

    private void Awake()
    {
        sprites.Add("A", spriteList[0]);
        sprites.Add("B", spriteList[1]);
        sprites.Add("X", spriteList[2]);
        sprites.Add("Y", spriteList[3]);
        sprites.Add("L", spriteList[4]);
        sprites.Add("R", spriteList[5]);
        sprites.Add("Up", spriteList[6]);
        sprites.Add("Down", spriteList[7]);
        sprites.Add("Left", spriteList[8]);
        sprites.Add("Right", spriteList[9]);
        colors.Add("A", colorList[0]);
        colors.Add("B", colorList[1]);
        colors.Add("X", colorList[2]);
        colors.Add("Y", colorList[3]);
        colors.Add("L", colorList[4]);
        colors.Add("R", colorList[5]);
        colors.Add("Up", colorList[6]);
        colors.Add("Down", colorList[7]);
        colors.Add("Left", colorList[8]);
        colors.Add("Right", colorList[9]);
    }

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
            var buttonRequestSprite = buttonRequestGameObject.GetComponent<SpriteRenderer>();
            var buttonRequestCombo = buttonRequestGameObject.GetComponent<ButtonRequestCombo>();
            var buttonRequestAnimator = buttonRequestGameObject.GetComponent<Animator>();
            buttonRequestSprite.sprite = sprites[buttonRequest.button];
            buttonRequestSprite.color = colors[buttonRequest.button];
            buttonRequestCombo.combo = buttonRequest.combo;
            buttonRequestCombo.isStart = buttonRequest.isComboStart;
            buttonRequestCombo.isEnd = buttonRequest.isComboEnd;
            buttonRequestGameObject.name = buttonRequestSprite.sprite.name;
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
