using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages which objects are active based on 
/// main menu state.
/// </summary>
public class MainMenuStateSystem : MonoBehaviour
{
    public MainMenuState CurrentState { get; private set; }

    private List<GameObject> MainObjects { get; set; }
    private List<GameObject> GameSelectionObjects { get; set; }
    private List<GameObject> LevelSelectionObjects { get; set; }
    private List<GameObject> OptionsObjects { get; set; }
    private List<GameObject> CreditsObjects { get; set; }

    #region Finding objects for each state

    private void Start()
    {
        GameplayStateManager.UpdateReferences();

        GetMainObjects();
        GetGameSelectionObjects();
        GetLevelSelectionObjects();
        GetOptionsObjects();
        GetCreditsObjects();

        SwitchTo(MainMenuState.Main);
    }

    private void GetMainObjects()
    {
        MainObjects = new List<GameObject>();

        MainObjects.Add(GameObject.Find("Main UI"));
    }

    private void GetGameSelectionObjects()
    {
        GameSelectionObjects = new List<GameObject>();

        GameSelectionObjects.Add(GameObject.Find("Game Selection UI"));
        SetGameObjectsActive(GameSelectionObjects, false);
    }

    private void GetLevelSelectionObjects()
    {
        LevelSelectionObjects = new List<GameObject>();

        LevelSelectionObjects.Add(GameObject.Find("Level Selection UI"));
        SetGameObjectsActive(LevelSelectionObjects, false);
    }

    private void GetOptionsObjects()
    {
        OptionsObjects = new List<GameObject>();

        OptionsObjects.Add(GameObject.Find("Options UI"));
        SetGameObjectsActive(OptionsObjects, false);
    }

    private void GetCreditsObjects()
    {
        CreditsObjects = new List<GameObject>();

        CreditsObjects.Add(GameObject.Find("Credits UI"));
        SetGameObjectsActive(CreditsObjects, false);
    }

    #endregion Finding objects for each state

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

            case MainMenuState.GameSelection:
                SwitchToGameSelection();
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

    #region Perform actions when switching to a state

    private void SwitchToMain()
    {
        SetGameObjectsActive(CreditsObjects, false);
        SetGameObjectsActive(OptionsObjects, false);
        SetGameObjectsActive(GameSelectionObjects, false);
        SetGameObjectsActive(MainObjects, true);
    }

    private void SwitchToGameSelection()
    {
        SetGameObjectsActive(MainObjects, false);
        SetGameObjectsActive(LevelSelectionObjects, false);
        SetGameObjectsActive(GameSelectionObjects, true);
    }

    private void SwitchToLevelSelection()
    {
        SetGameObjectsActive(GameSelectionObjects, false);
        SetGameObjectsActive(LevelSelectionObjects, true);
    }

    private void SwitchToOptions()
    {
        SetGameObjectsActive(OptionsObjects, true);
        SetGameObjectsActive(MainObjects, false);
    }

    private void SwitchToCredits()
    {
        SetGameObjectsActive(MainObjects, false);
        SetGameObjectsActive(CreditsObjects, true);
    }

    #endregion Perform actions when switching to a state

    private void SetGameObjectsActive(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            gameObjects[i].SetActive(active);
    }
}
