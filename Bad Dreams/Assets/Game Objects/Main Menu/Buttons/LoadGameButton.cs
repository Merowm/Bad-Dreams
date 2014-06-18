using UnityEngine;
using System.Collections;

public class LoadGameButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
