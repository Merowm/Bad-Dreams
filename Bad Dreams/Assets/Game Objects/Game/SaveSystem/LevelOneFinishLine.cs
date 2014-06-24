using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelOneFinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            Save save = SaveManager.CurrentSave;
            save.Levels[1].Locked = false;
            SaveManager.CurrentSave = save;
            PlayerPrefs.Save();
        }
    }
}
