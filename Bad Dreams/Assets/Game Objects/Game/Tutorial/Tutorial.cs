using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    UILabel tutorialTextBox;
    public string tutorialText;

	void Start () 
    {
        tutorialTextBox = GameObject.Find("Tutorial Text").GetComponent<UILabel>();
	}
	
	void Update () 
    {

	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (Input.GetButtonDown("Hide"))
            {
                tutorialTextBox.text = tutorialText;
                GameplayStateManager.SwitchTo(GameplayState.Tutorial);
            }
        }
    }
}
