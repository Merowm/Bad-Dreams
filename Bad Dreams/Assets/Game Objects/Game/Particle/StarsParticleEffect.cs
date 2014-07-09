using UnityEngine;
using System.Collections;
using SaveSystem;

public class StarsParticleEffect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ParticleSystem>().renderer.sortingLayerName = "Platform";
        particleSystem.emissionRate = SaveManager.CurrentSave.Drops;
    }
}
