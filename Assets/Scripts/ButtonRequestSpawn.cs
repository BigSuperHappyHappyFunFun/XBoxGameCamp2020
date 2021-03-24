using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonRequestSpawn : MonoBehaviour
{
    public LevelData levelData;
    public float time = 0;
    private QueueBox _queueBox;
    public List<GameObject> buttonRequestGameObjects = new List<GameObject>();
    public GameObject buttonRequestPrefab;

    private void Awake()
    {
        _queueBox = GetComponentInChildren<QueueBox>();
    }

    private void OnEnable()
    {
        _queueBox.AddCombos(CreateCombos());
        buttonRequestGameObjects.AddRange(AddButtonRequests());
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private List<Combo> CreateCombos()
    {
        var combos = new List<Combo>();
        var comboLabels = levelData.buttonRequests.Select(x => x.combo).Distinct().OrderBy(x => x);
        foreach (var comboLabel in comboLabels)
        {
            var combo = new Combo();
            combo.buttonRequests =
                new List<LevelData.ButtonRequest>(levelData.buttonRequests.Where(x => x.combo == comboLabel));
            combo.start = combo.buttonRequests.Min(x => x.time);
            combo.end = combo.buttonRequests.Max(x => x.time);
            combos.Add(combo);
        }

        return combos;
    }

    private IEnumerable<GameObject> AddButtonRequests()
    {
        var maxId = levelData.buttonRequests.Max(x => x.id);
        var buttonRequests = new GameObject[maxId + 1];
        for (var i = 0; i < buttonRequests.Length; i++)
        {
            var buttonRequest = levelData.buttonRequests.FirstOrDefault(x => x.id == i);
            if (buttonRequest == null) continue;
            var buttonRequestGameObject = Instantiate(buttonRequestPrefab, transform);
            var buttonRequestCombo = buttonRequestGameObject.GetComponent<ButtonRequestCombo>();
            var buttonRequestAnimator = buttonRequestGameObject.GetComponent<Animator>();
            var buttonRequestImage = buttonRequestGameObject.GetComponent<ButtonRequestImage>();
            var buttonRequestMoveTowards = buttonRequestGameObject.GetComponent<MoveTowards>();
            buttonRequestImage.ShowArrow(buttonRequest.button);
            buttonRequestCombo.combo = buttonRequest.combo;
            buttonRequestCombo.isStart = buttonRequest.isComboStart;
            buttonRequestCombo.isEnd = buttonRequest.isComboEnd;
            buttonRequestGameObject.name = buttonRequest.button;
            buttonRequestAnimator.enabled = false;
            buttonRequestGameObject.transform.localScale = Vector3.one;
            buttonRequestGameObject.SetActive(false);
            buttonRequests[i] = buttonRequestGameObject;
        }

        return buttonRequests;
    }
}