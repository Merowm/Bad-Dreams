using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelInfo : MonoBehaviour
{
    public string levelName;
    public int levelIndex; // first level (tutorial) is 0
    public int numberOfDrops;

    private void Start()
    {
        Collectible[] collectibles = GameObject.FindObjectsOfType<Collectible>();
        
        for (int i = 0; i < collectibles.Length; ++i)
        {
            if (collectibles[i].name == "Points")
            {
                ++numberOfDrops;
            }
        }
    }
}
