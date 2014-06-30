using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    GameObject tutorialTextBoxGameObject;
    UILabel tutorialTextBox;
    public string tutorialText;

	void Start () 
    {
        tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
        if (tutorialTextBoxGameObject != null)
        {
            tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
        }
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
                GameplayStateManager.SwitchTo(GameplayState.Tutorial);

                if (tutorialTextBoxGameObject == null)
                {
                    tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
                    tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
                    tutorialTextBox.text = tutorialText;
                }
                else
                tutorialTextBox.text = tutorialText;
            }
        }
    }
}
