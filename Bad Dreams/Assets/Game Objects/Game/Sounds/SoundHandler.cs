using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour 
{
    bool paused;

    AudioSource[] movementList;
    AudioSource droplet, treasure, time;

	AudioSource weaselMove, weaselPop, weaselVocal, spiderRattle;

    bool weaselMoving;

	void Start () 
    {
        weaselMoving = false;
        movementList = GameObject.Find("Level/Sounds/Movement").GetComponentsInChildren<AudioSource>();

        treasure = GameObject.Find("Level/Sounds/Treasure/teddybear").GetComponentInChildren<AudioSource>();
        droplet = GameObject.Find("Level/Sounds/Droplet/waterdroplet").GetComponentInChildren<AudioSource>();
        time = GameObject.Find("Level/Sounds/Time/addtime").GetComponentInChildren<AudioSource>();

		weaselMove = GameObject.Find("Level/Sounds/Weasel/Move").GetComponentInChildren<AudioSource>();
		weaselPop = GameObject.Find("Level/Sounds/Weasel/Pop").GetComponentInChildren<AudioSource>();
		weaselVocal = GameObject.Find("Level/Sounds/Weasel/Vocal").GetComponentInChildren<AudioSource>();
		
		spiderRattle = GameObject.Find("Level/Sounds/Spider/Rattle").GetComponentInChildren<AudioSource>();
	}

    void Update()
    {
        if (Time.timeScale == 0)
        {
            paused = true;
            AudioListener.pause = true;
        }

        if (paused)
        {
            if (Time.timeScale != 0)
            {
                paused = false;
                AudioListener.pause = false;
            }
        }
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

				//spider
			case SoundType.SpiderRattle:
                if (!spiderRattle.isPlaying)
                    spiderRattle.Play();
				break;

				//weasel
			case SoundType.WeaselMove:
                if (!weaselMoving)
                {
                    weaselMoving = true;
                    weaselMove.Play();
                    StartCoroutine(FadeIn(weaselMove, 0.5f));
                }
				break;
			
			case SoundType.WeaselPop:
                if (!weaselPop.isPlaying && !weaselVocal.isPlaying)
                {
                    weaselPop.Play();
                    weaselVocal.Play();
                }
				break;

			case SoundType.WeaselVocal:
				weaselVocal.Play();
				break;
        }
    }

    public void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.WeaselMove:
                if (weaselMoving)
                {
                    weaselMoving = false;
                    StartCoroutine(FadeOut(weaselMove, 0.5f));
                }
                break;
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float time)
    {
        for (float t = audioSource.volume; t > 0.0f; t -= Time.deltaTime / time)
        {
            audioSource.volume = t;
            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();
    }

    IEnumerator FadeIn(AudioSource audioSource, float time)
    {
        for (float t = audioSource.volume; t <= 1.0f; t += Time.deltaTime / time)
        {
            audioSource.volume = t;
            yield return new WaitForEndOfFrame();
        }
    }
}
