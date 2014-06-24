using UnityEngine;
using System.Collections;
using Saving;

public class LevelButton : MonoBehaviour
{
    private UILabel label;

    private void Start()
    {
        label = GetComponentInChildren<UILabel>();

        if (SaveManager.CurrentSave != null)
        {
            if (SaveManager.CurrentSave.Levels[0].Locked)
                label.text = "LOCKED";
            else
                label.text = "NOT LOCKED";
        }
    }

    private void OnClick()
    {
        Application.LoadLevel(label.text);
    }
}
