using UnityEngine;
using System.Collections;

public class RetryLevel : MonoBehaviour
{
    private void OnClick()
    {
        Application.LoadLevel("leveltest");
    }
}
