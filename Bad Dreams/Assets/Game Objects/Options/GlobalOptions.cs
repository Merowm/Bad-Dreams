using UnityEngine;
using System.Collections;

public class GlobalOptions : MonoBehaviour
{
    float soundLevel, musicLevel;

	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        soundLevel = 1.0f;
        musicLevel = 1.0f;
	}

    public void UpdateMusicLevel()
    {
        musicLevel = GameObject.Find("Music Slider").GetComponent<UISlider>().value;
        musicLevel = Mathf.Clamp(musicLevel, 0.0f, 1.0f);
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        foreach (GameObject music in musics)
        {
            music.audio.volume = musicLevel;
        }
    }

    public void UpdateSoundLevel()
    {
        soundLevel = GameObject.Find("Sound Slider").GetComponent<UISlider>().value;
        soundLevel = Mathf.Clamp(soundLevel, 0.0f, 1.0f);
        AudioListener.volume = soundLevel;
    }
}
