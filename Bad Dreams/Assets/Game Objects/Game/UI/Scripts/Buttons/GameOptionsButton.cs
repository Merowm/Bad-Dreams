using UnityEngine;
using System.Collections;

public class GameOptionsButton : MonoBehaviour 
{
    private void OnClick()
    {
        Debug.Log("test");
        GameplayStateManager.SwitchTo(GameplayState.Options);
    }
}
