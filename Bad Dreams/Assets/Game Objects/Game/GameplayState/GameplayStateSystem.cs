using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayStateSystem : MonoBehaviour
{
    public GameplayState CurrentState { get; private set; }

    private List<GameObject> GameOverObjects { get; set; }

    private void Start()
    {
        SwitchTo(GameplayState.Playing);
        GameplayStateManager.UpdateReferences();

        GetGameOverObjects();
    }

    private void GetGameOverObjects()
    {
        GameOverObjects = new List<GameObject>();
        GameOverObjects.Add(GameObject.Find("Game Over Menu"));
        SetGameObjectsActive(GameOverObjects, false);
    }

    public void SwitchTo(GameplayState state)
    {
        CurrentState = state;
        OnSwitch(state);
    }

    private void OnSwitch(GameplayState state)
    {
        switch (state)
        {
            case GameplayState.GameOver:
                SwitchToGameOver();
                break;

            case GameplayState.Playing:
                SwitchToPlaying();
                break;
        }
    }

    private void SwitchToGameOver()
    {
        SetGameObjectsActive(GameOverObjects, true);
        Time.timeScale = 0.0F;
    }

    private void SwitchToPlaying()
    {
        Time.timeScale = 1.0F;
    }

    private void SetGameObjectsActive(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            gameObjects[i].SetActive(active);
    }
}
