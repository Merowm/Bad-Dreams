using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelThreeButton : MonoBehaviour
{
    private bool Locked { get; set; }
    private UILabel label;
    private UIButton button;

    private void Start()
    {
        label = GetComponentInChildren<UILabel>();
        button = GetComponentInChildren<UIButton>();

        UpdateLabel();
    }

    private void Update()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (SaveManager.CurrentSave != null)
        {
            if (SaveManager.CurrentSave.Levels[2].Locked)
            {
                Locked = true;
                label.text = "Locked";
                button.enabled = false;
            }
            else
            {
                Locked = false;
                label.text = "Level 3";
            }
        }
    }

    private void OnClick()
    {
        if (!Locked)
            Application.LoadLevel("leveltest");
    }
}
