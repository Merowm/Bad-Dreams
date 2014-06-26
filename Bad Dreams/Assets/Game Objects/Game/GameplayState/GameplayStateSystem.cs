using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayStateSystem : MonoBehaviour
{
    public GameplayState CurrentState { get; private set; }

    private List<GameObject> PauseObjects { get; set; }
    private List<GameObject> GameOverObjects { get; set; }
    private List<GameObject> OptionsObjects { get; set; }

    #region Find objects for each state

    private void Start()
    {
        GameplayStateManager.UpdateReferences();
        GetPauseObjects();
        GetGameOverObjects();
        GetOptionsObjects();

        SwitchTo(GameplayState.Playing);
    }

    private void GetPauseObjects()
    {
        PauseObjects = new List<GameObject>();

        PauseObjects.Add(GameObject.Find("Pause Menu"));
        SetGameObjectsActive(PauseObjects, false);
    }

    private void GetGameOverObjects()
    {
        GameOverObjects = new List<GameObject>();

        GameOverObjects.Add(GameObject.Find("Game Over"));
        SetGameObjectsActive(GameOverObjects, false);
    }

    private void GetOptionsObjects()
    {
        OptionsObjects = new List<GameObject>();

        OptionsObjects.Add(GameObject.Find("Options UI"));
        SetGameObjectsActive(OptionsObjects, false);
    }

    #endregion Find objects for each state

    public void SwitchTo(GameplayState state)
    {
        OnSwitch(state);
    }

    private void OnSwitch(GameplayState state)
    {
		if (CurrentState != state)
		{
			switch (state)
			{
				case GameplayState.GameOver:
					SwitchToGameOver();
					break;

				case GameplayState.Paused:
					SwitchToPaused();
					break;

				case GameplayState.Playing:
					SwitchToPlaying();
					break;

				case GameplayState.Options:
					SwitchToOptions();
					break;
			}
		}
    }

    #region Perform actions when switching to a state

    private void SwitchToGameOver()
    {
		Debug.Log("SwitchToGameOver");
        SetGameObjectsActive(GameOverObjects, true);
        Invoke("LoadLastCheckpoint", 2.5F);
    }

    private void SwitchToPaused()
    {
        Time.timeScale = 0.0F;
        SetGameObjectsActive(OptionsObjects, false);
        SetGameObjectsActive(PauseObjects, true);
    }

    private void SwitchToPlaying()
    {
        Time.timeScale = 1.0F;
        SetGameObjectsActive(PauseObjects, false);
    }

    private void SwitchToOptions()
    {
        SetGameObjectsActive(PauseObjects, false);
        SetGameObjectsActive(OptionsObjects, true);
    }

    #endregion Perform actions when switching to a state

    private void SetGameObjectsActive(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            gameObjects[i].SetActive(active);
    }

    private void LoadLastCheckpoint()
    {
        //Application.LoadLevel("leveltest");
		GameObject player = GameObject.Find("Player");
		if (player)
		{
			player.GetComponent<Player>().GotoLastCheckpoint();
			SwitchTo(GameplayState.Playing);

			GameObject.Find("Timer").GetComponent<Timer>().Reset();
			//GameObject.Find("Game Over").GetComponent<TweenAlpha>().enabled = true;
			//GameObject.Find("Game Over").GetComponent<TweenAlpha>().ResetToBeginning();
			//GameObject.Find("Game Over").GetComponent<TweenAlpha>().enabled = false;
		}
    }
}
