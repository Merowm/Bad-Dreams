using UnityEngine;
using System.Collections;
using SaveSystem;

public class LevelButton : MonoBehaviour
{
    private bool locked;

    private UIButton button;
    private UILabel title;
    private UISprite collectibleOne;
    private UISprite collectibleTwo;
    private UISprite collectibleThree;
    private UILabel loadedLevelLabel;
    private UILabel levelIndexLabel;

    private Color32 collectibleFoundColor;
    private Color32 collectibleNotFoundColor;

    private void Start()
    {
        button = GetComponent<UIButton>();
        title = transform.FindChild("Title").GetComponent<UILabel>();
        collectibleOne = transform.FindChild("Collectible 1").GetComponent<UISprite>();
        collectibleTwo = transform.FindChild("Collectible 2").GetComponent<UISprite>();
        collectibleThree = transform.FindChild("Collectible 3").GetComponent<UISprite>();
        loadedLevelLabel = transform.FindChild("Level Name").GetComponent<UILabel>();
        levelIndexLabel = transform.FindChild("Level Index").GetComponent<UILabel>();
        collectibleNotFoundColor = new Color32(255, 255, 255, 50);
        collectibleFoundColor = new Color32(255, 255, 255, 255);

        UpdateLabel();
    }

    private void Update()
    {
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (SaveManager.CurrentSave != null)
        {
            int index;
            int.TryParse(levelIndexLabel.text, out index);

            if (SaveManager.CurrentSave.Levels[index].Locked)
            {
                locked = true;
                title.text = "Locked";
                button.enabled = false;
            }
            else
            {
                locked = false;
                title.text = "Level " + index;
            }

            if (!SaveManager.CurrentSave.Levels[index].Collectibles[0])
                collectibleOne.color = collectibleNotFoundColor;
            else
                collectibleOne.color = collectibleFoundColor;

            if (!SaveManager.CurrentSave.Levels[index].Collectibles[1])
                collectibleTwo.color = collectibleNotFoundColor;
            else
                collectibleTwo.color = collectibleFoundColor;

            if (!SaveManager.CurrentSave.Levels[index].Collectibles[2])
                collectibleThree.color = collectibleNotFoundColor;
            else
                collectibleThree.color = collectibleFoundColor;
        }
    }

    private void OnClick()
    {
        if (!locked)
        {
            Transition transition = GameObject.Find("Transition").GetComponent<Transition>();
            transition.PlayForward();
            transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadLevel"));
        }
    }

    private void LoadLevel()
    {
        Application.LoadLevel(loadedLevelLabel.text);
    }
}
