using UnityEngine;
using System.Collections;

public class MainMenuPlayButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.GameSelection);
    }
}
