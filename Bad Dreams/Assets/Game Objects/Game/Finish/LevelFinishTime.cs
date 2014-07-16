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

    private void OnEnable()
    {
        finalTime = GameObject.Find("Timer").GetComponent<Timer>().TimePassed;
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
        Invoke("StartCounting", 1.5F);
    }

    private void Update()
    {
        if (countTime)
        {
            UpdateClockLabel();
        }
    }

    private float timer = 0.0F;
    private float interval = 0.04F;

    private void UpdateClockLabel()
    {
        if (counter >= finalTime)
        {
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
}
