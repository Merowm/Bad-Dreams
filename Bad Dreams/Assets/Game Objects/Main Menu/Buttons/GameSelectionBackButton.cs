using UnityEngine;
using System.Collections;

public class GameSelectionBackButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.Main);
    }
}
