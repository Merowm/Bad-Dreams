using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuStateSystem : MonoBehaviour
{
    public MainMenuState CurrentState { get; private set; }

    private List<GameObject> MainObjects { get; set; }
    private List<GameObject> CreditsObjects { get; set; }

    private void Start()
    {
        GameplayStateManager.UpdateReferences();

        GetMainObjects();
        GetCreditsObjects();

        SwitchTo(MainMenuState.Main);
    }

    private void GetMainObjects()
    {
        MainObjects = new List<GameObject>();

        MainObjects.Add(GameObject.Find("Main UI"));
    }

    private void GetCreditsObjects()
    {
        CreditsObjects = new List<GameObject>();

        CreditsObjects.Add(GameObject.Find("Credits UI"));
    }

    public void SwitchTo(MainMenuState state)
    {
        CurrentState = state;
        OnSwitch(state);
    }

    private void OnSwitch(MainMenuState state)
    {
        switch (state)
        {
            case MainMenuState.Main:
                SwitchToMain();
                break;

            case MainMenuState.LevelSelection:
                SwitchToLevelSelection();
                break;

            case MainMenuState.Options:
                SwitchToOptions();
                break;

            case MainMenuState.Credits:
                SwitchToCredits();
                break;
        }
    }

    private void SwitchToMain()
    {
        SetGameObjectsActive(CreditsObjects, false);
        SetGameObjectsActive(MainObjects, true);
    }

    private void SwitchToLevelSelection()
    {
    }

    private void SwitchToOptions()
    {
    }

    private void SwitchToCredits()
    {
        SetGameObjectsActive(MainObjects, false);
        SetGameObjectsActive(CreditsObjects, true);
    }

    private void SetGameObjectsActive(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            gameObjects[i].SetActive(active);
    }
}
