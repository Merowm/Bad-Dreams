using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class GameplayStateManager
{
    private static GameplayStateSystem GameplayStateSystem { get; set; }

    static GameplayStateManager()
    {
        GameplayStateSystem = GameObject.FindObjectOfType<GameplayStateSystem>();
    }

    public static void SwitchTo(GameplayState gameplayState)
    {
        GameplayStateSystem.SwitchTo(gameplayState);
    }

    public static void UpdateReferences()
    {
        GameplayStateSystem = GameObject.FindObjectOfType<GameplayStateSystem>();
    }
}
