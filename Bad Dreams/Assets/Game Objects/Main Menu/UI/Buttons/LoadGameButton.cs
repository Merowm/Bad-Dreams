using UnityEngine;
using System.Collections;

public class LoadGameButton : MonoBehaviour
{
    private UIButton button;
    private bool statusChecked;

    private void Start()
    {
        button = GetComponent<UIButton>();
    }

    private void Update()
    {
        if (!statusChecked)
        {
            if (!PlayerPrefs.HasKey("Save"))
                button.isEnabled = false;
            else
                button.isEnabled = true;

            statusChecked = true;
        }
    }

    private void OnClick()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
        }
    }

    private void OnEnable()
    {
        statusChecked = false;
    }
}
