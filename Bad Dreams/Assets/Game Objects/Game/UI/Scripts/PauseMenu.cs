﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameplayStateManager.CurrentState == GameplayState.Playing)
                GameplayStateManager.SwitchTo(GameplayState.Paused);
            else if (GameplayStateManager.CurrentState == GameplayState.Paused)
                GameplayStateManager.SwitchTo(GameplayState.Playing);
        }
    }
}
