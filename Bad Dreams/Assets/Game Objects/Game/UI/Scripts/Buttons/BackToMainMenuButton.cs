using UnityEngine;
using System.Collections;

public class BackToMainMenuButton : MonoBehaviour
{
    private void OnClick()
    {
        Transition transition = GameObject.Find("Transition").GetComponent<Transition>();
        transition.PlayForward();
        transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadMainMenu"));
    }

    private void LoadMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
}
