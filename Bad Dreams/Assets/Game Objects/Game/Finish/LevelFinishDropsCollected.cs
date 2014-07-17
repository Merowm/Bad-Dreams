using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelFinishDropsCollected : MonoBehaviour
{
    public bool countDownFinished { get; set; }
    public bool newRecord { get; set; }

    private UILabel uiLabel;
    private LevelInfo levelInfo;
    private GameObject newRecordSprite;
    private bool countDrops;
    private int counter;
    private int totalDrops;
    private int maxDrops;
    private int oldDropRecord;

    private void OnEnable()
    {
        Time.timeScale = 1.0F;
        uiLabel = GetComponentInChildren<UILabel>();
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        DropCounter dropCounter = GameObject.Find("Drop Counter").GetComponent<DropCounter>();
        newRecordSprite = transform.FindChild("New Record").gameObject;
        totalDrops = dropCounter.DropCount;
        if (totalDrops > oldDropRecord && GameplayStateManager.CurrentState == GameplayState.LevelFinished)
        {
            newRecord = true;
        }
        uiLabel.text = 0 + " / " + levelInfo.numberOfDrops;
        maxDrops = levelInfo.numberOfDrops;
        counter = 0;
        countDrops = false;
        Invoke("StartCounting", 1.5F);
    }

    private void Start()
    {
        oldDropRecord = SaveManager.CurrentSave.Levels[levelInfo.levelIndex].DropsCollected;
    }

    private void Update()
    {
        if (countDrops)
        {
            UpdateDropsLabel();
        }
    }

    private float timer = 0.0F;
    private float interval = 0.07F;

    private void UpdateDropsLabel()
    {
        if (counter >= totalDrops)
        {
            countDrops = false;
            countDownFinished = true;
            if (newRecord)
            {
                newRecordSprite.SetActive(true);
            }
            return;
        }

        timer += Time.deltaTime;
        if (counter < totalDrops)
        {
            ++counter;
            uiLabel.text = counter.ToString() + " / " + maxDrops;
            timer = 0.0F;
        }
    }

    public void StartCounting()
    {
        countDrops = true;
    }
}
