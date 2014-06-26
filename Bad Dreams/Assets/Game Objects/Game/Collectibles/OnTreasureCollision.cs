using UnityEngine;
using System.Collections;
using SaveSystem;

public class OnTreasureCollision : MonoBehaviour
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

            if (gameObject.name == "Treasure 1")
            {
                save.Levels[levelInfo.levelIndex].Collectibles[0] = true;
            }
            else if (gameObject.name == "Treasure 2")
            {
                save.Levels[levelInfo.levelIndex].Collectibles[1] = true;
            }
            else if (gameObject.name == "Treasure 3")
            {
                save.Levels[levelInfo.levelIndex].Collectibles[2] = true;
            }

            SaveManager.CurrentSave = save;
            PlayerPrefs.Save();
        }
    }
}
