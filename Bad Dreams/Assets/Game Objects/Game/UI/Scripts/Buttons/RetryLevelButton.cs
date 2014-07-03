using UnityEngine;
using System.Collections;

public class RetryLevelButton : MonoBehaviour
{
    private Transition transition;

    private void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }

    private void OnClick()
    {
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "ReloadLevel"));
    }

    private void ReloadLevel()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "ReloadLevel"));
        LevelInfo levelInfo = GameObject.Find("LevelInfo").GetComponent<LevelInfo>();
        Application.LoadLevel(levelInfo.levelName);
        transition.PlayReverse();
    }
}
