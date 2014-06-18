using UnityEngine;
using System.Collections;

public class QuitGameButton : MonoBehaviour
{
    private void OnClick()
    {
        Application.Quit();
    }
}
