using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SaveSystem;

public class TreasureSpawner : MonoBehaviour
{
    private List<GameObject> treasures;
    private LevelInfo levelInfo;

    private void Start()
    {
        levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        treasures = new List<GameObject>();
        treasures.Add(transform.FindChild("Treasure 1").gameObject);
        treasures.Add(transform.FindChild("Treasure 2").gameObject);
        treasures.Add(transform.FindChild("Treasure 3").gameObject);

        DisableFoundTreasures();
    }

    private void DisableFoundTreasures()
    {
        Save save = SaveManager.CurrentSave;

        if (save.Levels[levelInfo.levelIndex].Collectibles[0] == true)
            treasures[0].SetActive(false);
        if (save.Levels[levelInfo.levelIndex].Collectibles[1] == true)
            treasures[1].SetActive(false);
        if (save.Levels[levelInfo.levelIndex].Collectibles[2] == true)
            treasures[2].SetActive(false);
    }
}
