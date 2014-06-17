using UnityEngine;
using System.Collections;

public class MainMenuPlayButton : MonoBehaviour
{
    private void OnClick()
    {
        Application.LoadLevel("leveltest");
    }
}
