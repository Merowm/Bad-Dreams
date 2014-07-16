using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelFinishTime : MonoBehaviour
{
    public bool countDownFinished;
    public bool newRecord;

    private UILabel uiLabel;
    private LevelInfo levelInfo;
    private int finalTime;
    private int counter;
    private bool countTime;
    private LevelFinishDropsCollected levelFinishDrops;
    private GameObject continueButton;

    private void OnEnable()
    {
        finalTime = GameObject.Find("Timer").GetComponent<Timer>().TimePassed;
        levelFinishDrops = GameObject.Find("Drops Collected").GetComponent<LevelFinishDropsCollected>();
        SetTimeCounterSpeed();
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        int oldBestTime = SaveManager.CurrentSave.Levels[levelInfo.levelIndex].BestTime;
        if (finalTime < oldBestTime)
        {
            newRecord = true;
        }
    }

    private void Start()
    {
        Time.timeScale = 1.0F;
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
                Invoke("StartCounting", 2.0F);
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
            interval = 0.04F;
        if (finalTime > 60)
            interval = 0.02F;
        if (interval > 120)
            interval = 0.01F;
    }
}
