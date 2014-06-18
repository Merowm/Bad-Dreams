using UnityEngine;
using System.Collections;

public class RetryLevelButton : MonoBehaviour
{
    private void OnClick()
    {
        Application.LoadLevel("leveltest");
    }
}
