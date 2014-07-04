using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelOneFinishLine : MonoBehaviour
{
    private LevelInfo levelInfo;

    private void Start()
    {
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Save save = SaveManager.CurrentSave;

            // make generic based on level info?
            save.Levels[1].Locked = false;

            int drops = GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount;
            save.Drops += drops;

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
