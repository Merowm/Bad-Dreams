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

            save.Levels[1].Locked = false;

            int drops = GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount;
            save.Drops += drops;

            SaveManager.CurrentSave = save;
            PlayerPrefs.Save();
            GameplayStateManager.SwitchTo(GameplayState.LevelFinished);
        }
    }
}
