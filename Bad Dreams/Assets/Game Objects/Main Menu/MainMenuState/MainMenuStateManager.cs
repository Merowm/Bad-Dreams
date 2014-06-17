using UnityEngine;
using System.Collections;

public class MainMenuStateManager : MonoBehaviour
{
    private static MainMenuStateSystem MainMenuStateSystem { get; set; }

    static MainMenuStateManager()
    {
        MainMenuStateSystem = GameObject.FindObjectOfType<MainMenuStateSystem>();
    }

    public static void SwitchTo(MainMenuState mainMenuState)
    {
        MainMenuStateSystem.SwitchTo(mainMenuState);
    }

    public static void UpdateReferences()
    {
        MainMenuStateSystem = GameObject.FindObjectOfType<MainMenuStateSystem>();
    }
}
