using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour 
{
    GameObject tutorialTextBoxGameObject;
    UILabel tutorialTextBox;
    public string tutorialText;
    bool insideTutorialCollider, triggered;
    Animator anim;

    bool InsideTutorialCollider
    {
        get { return insideTutorialCollider; }
        set 
        {
            if (value != insideTutorialCollider)
            {
                insideTutorialCollider = value;
                anim.SetBool("over", value);
            }
        }
    }

	void Start () 
    {
        triggered = false;
        if (PlayerPrefs.GetInt("toggletutorial", 0) == 0) 
        {
            triggered = true; //don't have tutorial signs pop up automatically
        }
        
        anim = GetComponent<Animator>();

        tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
        if (tutorialTextBoxGameObject != null)
        {
            tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
        }
	}

    void FixedUpdate()
    {
        InsideTutorialCollider = false;
    }
	
	void Update () 
    {
        if (insideTutorialCollider)
        {
            if (!triggered)
            {
                triggered = true;
                GameplayStateManager.SwitchTo(GameplayState.Tutorial);
                ChangeTutorialText(tutorialText);
            }

            if (Input.GetButtonDown("Hide"))
            {
                if (GameplayStateManager.CurrentState == GameplayState.Playing)
                {
                    GameplayStateManager.SwitchTo(GameplayState.Tutorial);
                    ChangeTutorialText(tutorialText);
                }
                else if (GameplayStateManager.CurrentState == GameplayState.Tutorial)
                {
                    GameplayStateManager.SwitchTo(GameplayState.Playing);
                }
            }
        }
	}

    void ChangeTutorialText(string text)
    {
        if (tutorialTextBoxGameObject == null)
        {
            tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
            tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
        }
        tutorialTextBox.text = text;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            InsideTutorialCollider = true;

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            InsideTutorialCollider = false;
        }
    }
}
