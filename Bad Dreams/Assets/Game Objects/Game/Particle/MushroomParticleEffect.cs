using UnityEngine;
using System.Collections;

public class MushroomParticleEffect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ParticleSystem>().renderer.sortingLayerName = "Pickups";
    }
}
