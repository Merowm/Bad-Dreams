using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    GameObject tutorialTextBoxGameObject;
    UILabel tutorialTextBox;
    public string tutorialText;
    bool insideTutorialCollider;

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
        if (insideTutorialCollider)
        {
            if (Input.GetButtonDown("Hide"))
            {
                if (GameplayStateManager.CurrentState == GameplayState.Playing)
                {
                    GameplayStateManager.SwitchTo(GameplayState.Tutorial);

                    if (tutorialTextBoxGameObject == null)
                    {
                        tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
                        tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
                    }
                    tutorialTextBox.text = tutorialText;
                }
                else if (GameplayStateManager.CurrentState == GameplayState.Tutorial)
                {
                    GameplayStateManager.SwitchTo(GameplayState.Playing);
                }
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            insideTutorialCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            insideTutorialCollider = false;
        }
    }
}
