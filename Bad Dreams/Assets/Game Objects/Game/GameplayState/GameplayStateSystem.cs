using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayStateSystem : MonoBehaviour
{
    public GameplayState CurrentState { get; private set; }

    private List<GameObject> PauseObjects { get; set; }
    private List<GameObject> GameOverObjects { get; set; }
    private List<GameObject> OptionsObjects { get; set; }
    private List<GameObject> TutorialObjects { get; set; }
    private List<GameObject> LevelFinishedObjects { get; set; }
    private List<GameObject> LevelFailedObjects { get; set; }

    private Transition transition;

    #region Find objects for each state

    private void Start()
    {
        GameplayStateManager.UpdateReferences();
        GetPauseObjects();
        GetGameOverObjects();
        GetOptionsObjects();
        GetTutorialObjects();
        GetLevelFinishedObjects();
        GetLevelFailedObjects();

        Time.timeScale = 1.0F;
        transition = GameObject.Find("Transition").GetComponent<Transition>();

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

    private void GetTutorialObjects()
    {
        TutorialObjects = new List<GameObject>();

        TutorialObjects.Add(GameObject.Find("Tutorial Popup"));
        SetGameObjectsActive(TutorialObjects, false);
    }

    private void GetLevelFinishedObjects()
    {
        LevelFinishedObjects = new List<GameObject>();

        LevelFinishedObjects.Add(GameObject.Find("Level Finished"));
        SetGameObjectsActive(LevelFinishedObjects, false);
    }

    private void GetLevelFailedObjects()
    {
        LevelFailedObjects = new List<GameObject>();

        LevelFailedObjects.Add(GameObject.Find("Level Failed"));
        SetGameObjectsActive(LevelFailedObjects, false);
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
            CurrentState = state;

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

                case GameplayState.Tutorial:
                    SwitchToTutorial();
                    break;

                case GameplayState.LevelFinished:
                    SwitchToLevelFinished();
                    break;

                case GameplayState.LevelFailed:
                    SwitchToLevelFailed();
                    break;
			}
		}
    }

    #region Perform actions when switching to a state

    private void SwitchToGameOver()
    {
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadLastCheckpoint"));
    }

    private void SwitchToPaused()
    {
        Time.timeScale = 0.0F;
        SetGameObjectsActive(OptionsObjects, false);
        SetGameObjectsActive(PauseObjects, true);
        SetGameObjectsActive(TutorialObjects, false);
    }

    private void SwitchToPlaying()
    {
        Time.timeScale = 1.0F;
        SetGameObjectsActive(GameOverObjects, false);
        SetGameObjectsActive(PauseObjects, false);
        SetGameObjectsActive(TutorialObjects, false);
    }

    private void SwitchToOptions()
    {
        SetGameObjectsActive(PauseObjects, false);
        SetGameObjectsActive(OptionsObjects, true);
    }

    private void SwitchToTutorial()
    {
        Time.timeScale = 0.0F;
        SetGameObjectsActive(TutorialObjects, true);
    }

    private void SwitchToLevelFinished()
    {
        Time.timeScale = 0.0F;
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "ActivateLevelFinishScreen"));
    }

    private void SwitchToLevelFailed()
    {
        Time.timeScale = 0.0F;
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "ActivateLevelFailedScreen"));
    }

    #endregion Perform actions when switching to a state

    #region Game Over

    private void LoadLastCheckpoint()
    {
		GameObject player = GameObject.Find("Player");
		if (player)
		{
            transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "LoadLastCheckpoint"));
			player.GetComponent<Player>().GotoLastCheckpoint();
            player.GetComponent<HidingSkill>().Unhide();
			SwitchTo(GameplayState.Playing);
            transition.PlayReverse();
		}
    }

    #endregion Game Over

    #region Level Finished

    private void ActivateLevelFinishScreen()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "ActivateLevelFinishScreen"));
        SetGameObjectsActive(LevelFinishedObjects, true);
        transition.PlayReverse();
    }

    #endregion Level Finished

    #region Level Failed

    private void ActivateLevelFailedScreen()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "ActivateLevelFailedScreen"));
        SetGameObjectsActive(LevelFailedObjects, true);
        transition.PlayReverse();
    }

    #endregion Level Failed

    private void SetGameObjectsActive(List<GameObject> gameObjects, bool active)
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            gameObjects[i].SetActive(active);
    }
}
