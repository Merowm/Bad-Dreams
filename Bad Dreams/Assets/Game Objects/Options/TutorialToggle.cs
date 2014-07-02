using UnityEngine;
using System.Collections;

public class TutorialToggle : MonoBehaviour 
{

    UIToggle tutToggle;

	// Use this for initialization
	void Start () 
    {
        tutToggle = GameObject.Find("Toggle Tutorial").GetComponent<UIToggle>();
        tutToggle.value = IntToBool(PlayerPrefs.GetInt("toggletutorial", 1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateTutorialToggle()
    {
        PlayerPrefs.SetInt("toggletutorial", BoolToInt(tutToggle.value));
    }

    int BoolToInt(bool value)
    {
        if (value == true)
            return 1;
        if (value == false)
            return 0;
        else return -1;
    }

    bool IntToBool(int value)
    {
        if (value == 1)
            return true;
        if (value == 0)
            return false;
        return true;
    }
}
