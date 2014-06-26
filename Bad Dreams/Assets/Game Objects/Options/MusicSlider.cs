using UnityEngine;
using System.Collections;

public class MusicSlider : MonoBehaviour 
{
    UISlider music;
    const float defaultVolume = 1.0f;

    void Start()
    {
        music = GetComponent<UISlider>();
        music.value = PlayerPrefs.GetFloat("music", defaultVolume);
    }

    public void SaveMusicLevel()
    {
        PlayerPrefs.SetFloat("music", music.value);

        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        foreach (GameObject musicAsset in musics)
        {
            musicAsset.audio.volume = music.value;
        }
    }
}
