using UnityEngine;
using System.Collections;

public class BackToMainMenuButton : MonoBehaviour
{
    private void OnClick()
    {
        Application.LoadLevel("MainMenu");
    }
}
