using UnityEngine;
using System.Collections;
using SaveSystem;

public class TotalDropsMainMenu : MonoBehaviour
{
    private UILabel uiLabel;
    private bool statusUpdated;

    private void Start()
    {
        uiLabel = GetComponentInChildren<UILabel>();

        if (SaveManager.CurrentSave != null)
        {
            uiLabel.text = SaveManager.CurrentSave.Drops.ToString();
        }
    }

    private void Update()
    {
        if (!statusUpdated)
        {
            uiLabel.text = SaveManager.CurrentSave.Drops.ToString();
            statusUpdated = true;
        }
    }

    private void OnEnable()
    {
        statusUpdated = false;
    }
}
