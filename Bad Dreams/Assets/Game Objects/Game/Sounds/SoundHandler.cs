using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour 
{
    AudioSource[] movementList;
    AudioSource droplet, treasure, time;

	AudioSource[] batSqueak, dogGrowl;
	AudioSource batFlap, dogSnarl, weaselMove, weaselPop, weaselVocal, spiderRattle;

	void Start () 
    {
        movementList = GameObject.Find("Level/Sounds/Movement").GetComponentsInChildren<AudioSource>();

        treasure = GameObject.Find("Level/Sounds/Treasure/teddybear").GetComponentInChildren<AudioSource>();
        droplet = GameObject.Find("Level/Sounds/Droplet/waterdroplet").GetComponentInChildren<AudioSource>();
        time = GameObject.Find("Level/Sounds/Time/addtime").GetComponentInChildren<AudioSource>();

		batSqueak = GameObject.Find("Level/Sounds/Bat/Squeak").GetComponentsInChildren<AudioSource>();
		dogGrowl = GameObject.Find("Level/Sounds/Dog/Growl").GetComponentsInChildren<AudioSource>();

		batFlap = GameObject.Find("Level/Sounds/Bat/Flap").GetComponentInChildren<AudioSource>();
		dogSnarl = GameObject.Find("Level/Sounds/Dog/Snarl").GetComponentInChildren<AudioSource>();

		weaselMove = GameObject.Find("Level/Sounds/Weasel/Move").GetComponentInChildren<AudioSource>();
		weaselPop = GameObject.Find("Level/Sounds/Weasel/Pop").GetComponentInChildren<AudioSource>();
		weaselVocal = GameObject.Find("Level/Sounds/Weasel/Vocal").GetComponentInChildren<AudioSource>();
		
		spiderRattle = GameObject.Find("Level/Sounds/Spider/Rattle").GetComponentInChildren<AudioSource>();
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

				//bat
			case SoundType.BatFlap:
				batFlap.Play();
				break;

			case SoundType.BatSqueak:
				batSqueak[Random.Range(0, batSqueak.Length)].Play();
				break;

				//dog
			case SoundType.DogGrowl:
				dogGrowl[Random.Range(0, dogGrowl.Length)].Play();
				break;

			case SoundType.DogSnarl:
				dogSnarl.Play();
				break;

				//spider
			case SoundType.SpiderRattle:
				spiderRattle.Play();
				break;

				//weasel
			case SoundType.WeaselMove:
				weaselMove.Play();
				break;
			
			case SoundType.WeaselPop:
				weaselPop.Play();
				break;

			case SoundType.WeaselVocal:
				weaselVocal.Play();
				break;
        }
    }
}
