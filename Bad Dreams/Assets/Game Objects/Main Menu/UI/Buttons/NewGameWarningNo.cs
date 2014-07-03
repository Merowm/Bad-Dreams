using UnityEngine;
using System.Collections;

public class NewGameWarningNo : MonoBehaviour 
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.GameSelection);
    }
}
