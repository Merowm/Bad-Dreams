using UnityEngine;
using System.Collections;
using Saving;

public class NewGameButton : MonoBehaviour
{
    private void OnClick()
    {
        SaveManager.NewGame();
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
