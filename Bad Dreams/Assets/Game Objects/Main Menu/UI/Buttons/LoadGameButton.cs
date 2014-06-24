using UnityEngine;
using System.Collections;

public class LoadGameButton : MonoBehaviour
{
    private UIButton button;

    private void Start()
    {
        button = GetComponent<UIButton>();
    }

    private void Update()
    {
        if (!PlayerPrefs.HasKey("Save"))
            button.isEnabled = false;

    }

    private void OnClick()
    {
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
