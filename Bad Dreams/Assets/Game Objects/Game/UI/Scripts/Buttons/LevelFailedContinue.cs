using UnityEngine;
using System.Collections;

public class LevelFailedContinue : MonoBehaviour 
{
    private Transition transition;

    private void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }

    private void OnClick()
    {
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "SwitchToLevelSelect"));
    }

    private void SwitchToLevelSelect()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "SwitchToLevelSelect"));
        Application.LoadLevel("MainMenu");
        transition.PlayReverse();
    }
}
