using UnityEngine;
using System.Collections;

public class ContinueButton : MonoBehaviour
{
    private void OnClick()
    {
        GameplayStateManager.SwitchTo(GameplayState.Playing);
    }
}
