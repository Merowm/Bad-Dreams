using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    GameObject tutorialTextBoxGameObject;
    UILabel tutorialTextBox;
    public string tutorialText;
    bool insideTutorialCollider, tutorialsDisabled;

    bool InsideTutorialCollider
    {
        get { return insideTutorialCollider; }
        set
        {
            if (value != insideTutorialCollider)
            {
                insideTutorialCollider = value;
            }
        }
    }

    void Start()
    {
        tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
        if (tutorialTextBoxGameObject != null)
        {
            tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
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
            if (GameplayStateManager.CurrentState == GameplayState.Playing)
            {
                GameplayStateManager.SwitchTo(GameplayState.Tutorial);
                ChangeTutorialText(tutorialText);
            }
            InsideTutorialCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (insideTutorialCollider)
            {
                if (GameplayStateManager.CurrentState == GameplayState.Tutorial)
                {
                    GameplayStateManager.SwitchTo(GameplayState.Playing);
                }
            }
            InsideTutorialCollider = false;
        }
    }
}
