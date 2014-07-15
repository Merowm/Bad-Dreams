using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    GameObject tutorialTextBoxGameObject;
    UILabel tutorialTextBox, helpfulSlash;
    UISprite controllerImage, keyboardImage;

    public string controllerSprite, keyboardSprite;
    public string tutorialText;
    public bool controlImagesOn;
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
    }

    void ChangeTutorialText(string text)
    {
        if (tutorialTextBoxGameObject)
        {
            if (controlImagesOn)
            {
                helpfulSlash.text = "/";
            }
            else
                helpfulSlash.text = "";
            controllerImage.spriteName = controllerSprite;
            keyboardImage.spriteName = keyboardSprite;
            tutorialTextBox.text = text;
        }
        else
        {
            tutorialTextBoxGameObject = GameObject.Find("Tutorial Text");
            if (tutorialTextBoxGameObject)
            {
                tutorialTextBox = tutorialTextBoxGameObject.GetComponent<UILabel>();
                controllerImage = GameObject.Find("Tutorial Popup").transform.Find("Controller Image").GetComponent<UISprite>();
                keyboardImage = GameObject.Find("Tutorial Popup").transform.Find("Keyboard Image").GetComponent<UISprite>();
                helpfulSlash = GameObject.Find("Tutorial Popup").transform.Find("Helpful Forward Slash").GetComponent<UILabel>();
                ChangeTutorialText(tutorialText);
            }
            else
                Debug.Log("Something has gone horribly wrong, cannot find the tutorial text box");
        }
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
