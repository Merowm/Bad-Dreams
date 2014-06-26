using UnityEngine;
using System.Collections;

public class SoundSlider : MonoBehaviour {

    UISlider sound;
    const float defaultVolume = 1.0f;

	void Start() 
    {
        sound = GetComponent<UISlider>();
        sound.value = PlayerPrefs.GetFloat("sound", defaultVolume);
        AudioListener.volume = sound.value;
	}

    public void SaveSoundLevel()
    {
        PlayerPrefs.SetFloat("sound", sound.value);
        AudioListener.volume = sound.value;
    }
}
