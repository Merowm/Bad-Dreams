using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelFinishTime : MonoBehaviour
{
    public bool countDownFinished { get; set; }
    public bool newRecord { get; set; }
    public int oldBestTime { get; set; }
    public int finalTime { get; set; }

    private UILabel uiLabel;
    private LevelInfo levelInfo;
    private GameObject newRecordSprite;
    private int counter;
    private bool countTime;
    private LevelFinishDropsCollected levelFinishDrops;
    private GameObject continueButton;
    
    private void OnEnable()
    {
        levelFinishDrops = GameObject.Find("Drops Collected").GetComponent<LevelFinishDropsCollected>();
        newRecordSprite = transform.FindChild("New Record").gameObject;
        SetTimeCounterSpeed();
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
    }

    private void Start()
    {
        uiLabel = GetComponentInChildren<UILabel>();
        counter = 0;
        countTime = false;
        continueButton = GameObject.Find("Level Finished").transform.FindChild("Continue").gameObject;
    }

    private void Update()
    {
        if (levelFinishDrops.countDownFinished && countTime == false)
        {
            if (levelFinishDrops.newRecord == false)
                Invoke("StartCounting", 0.25F);
            else
                Invoke("StartCounting", 1.5F);
        }

        if (levelFinishDrops.countDownFinished && this.countDownFinished)
        {
            continueButton.SetActive(true);
        }

        if (countTime)
        {
            UpdateClockLabel();
        }
    }

    private float timer = 0.0F;
    private float interval = 0.05F;

    private void UpdateClockLabel()
    {
        if (counter >= finalTime)
        {
            if (newRecord)
            {
                newRecordSprite.SetActive(true);
            }
            countDownFinished = true;
            countTime = false;
            return;
        }

        timer += Time.deltaTime;
        if (timer > interval)
        {
            ++counter;
            uiLabel.text = counter.ToString();
            timer = 0.0F;
        }
    }

    private void StartCounting()
    {
        countTime = true;
    }

    private void SetTimeCounterSpeed()
    {
        if (finalTime > 30)
            interval = 0.03F;
        if (finalTime > 60)
            interval = 0.01F;
        if (finalTime > 120)
            interval = 0.005F;
        if (finalTime > 180)
            interval = 0.001F;
    }
}
