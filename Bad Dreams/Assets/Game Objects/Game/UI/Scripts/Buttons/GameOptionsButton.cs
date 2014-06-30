using UnityEngine;
using System.Collections;

public class GameOptionsButton : MonoBehaviour 
{
    private void OnClick()
    {
        GameplayStateManager.SwitchTo(GameplayState.Options);
    }
}
