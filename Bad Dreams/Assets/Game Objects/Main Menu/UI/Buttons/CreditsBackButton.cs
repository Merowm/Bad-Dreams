using UnityEngine;
using System.Collections;

public class CreditsBackButton : MonoBehaviour 
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.Main);
    }
}
