using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameplayStateManager.CurrentState == GameplayState.Playing || GameplayStateManager.CurrentState == GameplayState.Options)
                GameplayStateManager.SwitchTo(GameplayState.Paused);
            else if (GameplayStateManager.CurrentState == GameplayState.Paused)
                GameplayStateManager.SwitchTo(GameplayState.Playing);
            else if (GameplayStateManager.CurrentState == GameplayState.Tutorial)
                GameplayStateManager.SwitchTo(GameplayState.Paused);
        }
    }
}
