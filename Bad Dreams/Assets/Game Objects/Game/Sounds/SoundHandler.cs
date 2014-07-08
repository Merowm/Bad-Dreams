using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour 
{
    AudioSource[] movementList;
    AudioSource dogAlert, dogCollision, dogSeePlayer;
    AudioSource droplet, treasure, time;

	void Start () 
    {
        movementList = GameObject.Find("Level/Sounds/Movement").GetComponentsInChildren<AudioSource>();

        dogCollision = GameObject.Find("Level/Sounds/Dog/Collision Against Dog Sound").GetComponentInChildren<AudioSource>();
        dogSeePlayer = GameObject.Find("Level/Sounds/Dog/Dog See Player").GetComponentInChildren<AudioSource>();
        dogAlert = GameObject.Find("Level/Sounds/Dog/Dog Alert").GetComponentInChildren<AudioSource>();

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

                //dog
            case SoundType.DogCollision:
                dogCollision.Play();
                break;
            case SoundType.DogSeePlayer:
                if (dogAlert.isPlaying)
                    dogAlert.Stop();
                if (!dogSeePlayer.isPlaying)
                dogSeePlayer.Play();
                break;
            case SoundType.DogAlert:
                if (dogSeePlayer.isPlaying)
                    dogSeePlayer.Stop();
                if (!dogAlert.isPlaying)
                    dogAlert.Play();
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

    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.DogSeePlayer:
                if (dogSeePlayer.isPlaying)
                    dogSeePlayer.Stop();
                break;
            case SoundType.DogAlert:
                if (dogAlert.isPlaying)
                    dogAlert.Stop();
                break;
        }
    }
}
