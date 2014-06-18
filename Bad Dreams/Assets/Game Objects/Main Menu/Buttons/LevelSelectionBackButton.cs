using UnityEngine;
using System.Collections;

public class LevelSelectionBackButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.GameSelection);
    }
}
