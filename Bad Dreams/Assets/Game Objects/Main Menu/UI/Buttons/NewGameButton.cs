using UnityEngine;
using System.Collections;

public class NewGameButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
