using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelOneFinishLine : MonoBehaviour
{
    private LevelInfo levelInfo;
    private LevelFinishTime levelFinishTime;
    private LevelFinishDropsCollected levelFinishDrops;
    private bool triggered;

    private void OnEnable()
    {
        levelFinishDrops = GameObject.Find("Drops Collected").GetComponent<LevelFinishDropsCollected>();
        levelFinishTime = GameObject.Find("Level Finished").transform.FindChild("Time").GetComponent<LevelFinishTime>();
    }

    private void Start()
    {
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && !triggered)
        {
            triggered = true;

            // Inform level finish if new records
            levelFinishTime.oldBestTime = SaveManager.CurrentSave.Levels[levelInfo.levelIndex].BestTime;
            levelFinishTime.finalTime = GameObject.Find("Timer").GetComponent<Timer>().TimePassed;
            levelFinishTime.newRecord = (levelFinishTime.finalTime < levelFinishTime.oldBestTime) || levelFinishTime.oldBestTime == 0;

            levelFinishDrops.oldDropRecord = SaveManager.CurrentSave.Levels[levelInfo.levelIndex].DropsCollected;
            levelFinishDrops.totalDrops = GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount;
            Debug.Log(levelFinishDrops.totalDrops + ", " + levelFinishDrops.oldDropRecord);
            levelFinishDrops.newRecord = (levelFinishDrops.totalDrops > levelFinishDrops.oldDropRecord);

            Debug.Log("Drops: " + levelFinishDrops.newRecord + "; Time: " + levelFinishTime.newRecord);

            // Save new stuff
            Save save = SaveManager.CurrentSave;

            if ((levelInfo.levelIndex + 1 < save.Levels.Count))
            {
                save.Levels[levelInfo.levelIndex + 1].Locked = false;
            }

            int drops = GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount;
            save.Drops += drops;
            if (drops > save.Levels[levelInfo.levelIndex].DropsCollected)
            {
                save.Levels[levelInfo.levelIndex].DropsCollected = drops;
            }
            save.Levels[levelInfo.levelIndex].TotalDrops = levelInfo.numberOfDrops;


            int time = GameObject.Find("Timer").GetComponent<Timer>().TimePassed;
            if (time < save.Levels[levelInfo.levelIndex].BestTime ||
                save.Levels[levelInfo.levelIndex].BestTime == 0)
            {
                save.Levels[levelInfo.levelIndex].BestTime = time;
            }

            SaveManager.CurrentSave = save;
            PlayerPrefs.Save();
            GameplayStateManager.SwitchTo(GameplayState.LevelFinished);
        }
    }
}
