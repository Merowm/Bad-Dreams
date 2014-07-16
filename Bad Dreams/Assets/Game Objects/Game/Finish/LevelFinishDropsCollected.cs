using UnityEngine;
using System.Collections;

public class LevelFinishDropsCollected : MonoBehaviour
{
    private UILabel uiLabel;
    private bool countDrops;
    private int counter;
    private int totalDrops;
    private int maxDrops;

    private void OnEnable()
    {
        Time.timeScale = 1.0F;
        uiLabel = GetComponentInChildren<UILabel>();
        DropCounter dropCounter = GameObject.Find("Drop Counter").GetComponent<DropCounter>();
        totalDrops = dropCounter.DropCount;
        LevelInfo levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
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
    private float interval = 0.05F;

    private void UpdateDropsLabel()
    {
        if (counter >= totalDrops)
        {
            countDrops = false;
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
