using UnityEngine;
using System.Collections;

public class FinishLineParticleEffect : MonoBehaviour
{
    private void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.renderer.sortingLayerName = "Player Foreground";
        particleSystem.renderer.sortingOrder = -2;
    }
}
