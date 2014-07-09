using UnityEngine;
using System.Collections;
using SaveSystem;

public class NewGameWarningYes : MonoBehaviour
{
    private void OnClick()
    {
        GameObject.Find("Stars Particle Effect").GetComponent<ParticleSystem>().emissionRate = 0;
        SaveManager.NewGame();
        MainMenuStateManager.SwitchTo(MainMenuState.LevelSelection);
    }
}
