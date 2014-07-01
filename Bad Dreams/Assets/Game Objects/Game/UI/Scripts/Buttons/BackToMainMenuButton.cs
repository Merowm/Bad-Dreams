using UnityEngine;
using System.Collections;

public class BackToMainMenuButton : MonoBehaviour
{
    private Transition transition;

    private void Start()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }

    private void OnClick()
    {
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadMainMenu"));
    }

    private void LoadMainMenu()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "LoadMainMenu"));
        Application.LoadLevel("MainMenu");
        transition.PlayReverse();
    }
}
