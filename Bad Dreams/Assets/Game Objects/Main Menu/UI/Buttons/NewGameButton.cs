using UnityEngine;
using System.Collections;
using SaveSystem;

public class NewGameButton : MonoBehaviour
{
    private void OnClick()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            MainMenuStateManager.SwitchTo(MainMenuState.NewGameWarning);
        }
        else
        {
            GameObject.Find("Stars Particle Effect").GetComponent<ParticleSystem>().emissionRate = 0;
            SaveManager.NewGame();
            MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
        }
    }
}
