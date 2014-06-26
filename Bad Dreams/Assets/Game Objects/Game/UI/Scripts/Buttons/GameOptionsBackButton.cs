using UnityEngine;
using System.Collections;

public class GameOptionsBackButton : MonoBehaviour 
{
    private void OnClick()
    {
        GameplayStateManager.SwitchTo(GameplayState.Paused);
    }
}
