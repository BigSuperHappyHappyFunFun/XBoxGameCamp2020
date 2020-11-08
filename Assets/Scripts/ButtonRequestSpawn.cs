using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public Transform spawn;
    public Transform target;
    public LevelData levelData;
    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    public List<Sprite> spriteList = new List<Sprite>();
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
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (index >= levelData.buttonRequests.Count)
            return;

        var buttonRequest = levelData.buttonRequests[index];
        var targetDistance = Vector3.Distance(spawn.position, target.position);
        var timeToTarget = targetDistance / buttonRequest.speed;
        var startTime = buttonRequest.start - timeToTarget;
        var timeForNextButton = time >= startTime;
        if (timeForNextButton)
        {
            var buttonRequestGameObject = Instantiate(buttonRequestPrefab, spawn.position, Quaternion.identity, transform);
            var buttonRequestMove = buttonRequestGameObject.GetComponent<ButtonRequestMove>();
            var buttonRequestSprite = buttonRequestGameObject.GetComponent<SpriteRenderer>();
            var buttonRequestDelete = buttonRequestGameObject.GetComponent<ButtonRequestDelete>();
            buttonRequestMove.speed = buttonRequest.speed;
            buttonRequestSprite.sprite = sprites[buttonRequest.button];
            buttonRequestDelete.spawnPosition = spawn.position;
            buttonRequestGameObject.name = buttonRequestSprite.sprite.name;
            inputChecker.buttonRequests.Add(buttonRequestGameObject);
            index++;
        }
    }
}
