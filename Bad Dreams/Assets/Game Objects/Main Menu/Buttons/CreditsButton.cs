using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.Credits);
    }
}
