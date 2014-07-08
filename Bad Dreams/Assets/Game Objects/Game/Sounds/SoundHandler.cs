using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour 
{
    AudioSource[] movementList;
    AudioSource droplet, treasure, time;

	void Start () 
    {
        movementList = GameObject.Find("Level/Sounds/Movement").GetComponentsInChildren<AudioSource>();

        treasure = GameObject.Find("Level/Sounds/Treasure/teddybear").GetComponentInChildren<AudioSource>();
        droplet = GameObject.Find("Level/Sounds/Droplet/waterdroplet").GetComponentInChildren<AudioSource>();
        time = GameObject.Find("Level/Sounds/Time/addtime").GetComponentInChildren<AudioSource>();
	}

    public void PlaySound(SoundType type)
    {
        switch (type)
        {
                //player
            case SoundType.Movement:
                movementList[Random.Range(0, movementList.Length)].Play();
                break;

                //collectibles
            case SoundType.Treasure:
                treasure.Play();
                break;
            case SoundType.Droplet:
                droplet.Play();
                break;
            case SoundType.Time:
                time.Play();
                break;
        }
    }
}
