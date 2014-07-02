using UnityEngine;
using System.Collections;
using SaveSystem;

public class NewGameWarningYes : MonoBehaviour
{
    private void OnClick()
    {
        SaveManager.NewGame();
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
