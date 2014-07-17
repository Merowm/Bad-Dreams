using UnityEngine;
using System.Collections;
using SaveSystem;

public class StarsParticleEffect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ParticleSystem>().renderer.sortingLayerName = "Platform";

        if (SaveManager.CurrentSave != null)
        {
            for (int i = 0; i < SaveManager.CurrentSave.Levels.Count; ++i)
            {
                particleSystem.emissionRate += SaveManager.CurrentSave.Levels[i].DropsCollected / 2;
            }
        }
    }
}
