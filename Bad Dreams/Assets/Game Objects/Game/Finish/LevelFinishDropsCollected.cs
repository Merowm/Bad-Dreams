using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelFinishDropsCollected : MonoBehaviour
{
    public bool countDownFinished { get; set; }
    public bool newRecord { get; set; }
    public int oldDropRecord { get; set; }
    public int totalDrops { get; set; }

    private UILabel uiLabel;
    private LevelInfo levelInfo;
    private GameObject newRecordSprite;
    private bool countDrops;
    private int counter;
    private int maxDrops;

    private void OnEnable()
    {
        Time.timeScale = 1.0F;
        uiLabel = GetComponentInChildren<UILabel>();
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        newRecordSprite = transform.FindChild("New Record").gameObject;
        uiLabel.text = 0 + " / " + levelInfo.numberOfDrops;
        maxDrops = levelInfo.numberOfDrops;
        counter = 0;
        countDrops = false;
        Invoke("StartCounting", 1.5F);
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
