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

        treasure = GameObject.Find("Level/Sounds/Treasure/teddybear").GetComponentInChildren<AudioSource>();
        droplet = GameObject.Find("Level/Sounds/Droplet/waterdroplet").GetComponentInChildren<AudioSource>();
        time = GameObject.Find("Level/Sounds/Time/addtime").GetComponentInChildren<AudioSource>();
	}

    public void PlaySound(string type)
    {
        switch (type)
        {
            case "movement":
                movementList[Random.Range(0, movementList.Length)].Play();
                break;
            case "dogcollision":
                dogCollision.Play();
                break;
            case "treasure":
                treasure.Play();
                break;
            case "droplet":
                droplet.Play();
                break;
            case "time":
                time.Play();
                break;
        }
    }
}
