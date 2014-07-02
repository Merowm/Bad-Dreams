using UnityEngine;
using System.Collections;
using SaveSystem;

public class NewGameButton : MonoBehaviour
{
    private void OnClick()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            MainMenuStateManager.SwitchTo(MainMenuState.NewGameWarning);
        }
        else
        {
            SaveManager.NewGame();
            MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
        }
    }
}
