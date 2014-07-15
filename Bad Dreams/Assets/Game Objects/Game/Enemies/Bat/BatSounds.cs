using UnityEngine;
using System.Collections;

public class BatSounds : MonoBehaviour 
{
    AudioSource batSlap;
    AudioSource[] batSqueak;

	void Start () 
    {
        batSlap = transform.Find("Bat Sounds/Bat Flap").GetComponent<AudioSource>();
        batSqueak = transform.Find("Bat Sounds/Bat Squeak").GetComponentsInChildren<AudioSource>();
	}

    public void BatFlap()
    {
        batSlap.Play();
    }

    public void BatSqueak()
    {
        for (int i = 0; i < batSqueak.Length; ++i)
        {
            if (batSqueak[i].isPlaying)
                return;
        }

        batSqueak[Random.Range(0, batSqueak.Length)].Play();
    }
}
