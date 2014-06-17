using UnityEngine;
using System.Collections;

public class OptionsButton : MonoBehaviour
{
    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.Options);
    }
}
