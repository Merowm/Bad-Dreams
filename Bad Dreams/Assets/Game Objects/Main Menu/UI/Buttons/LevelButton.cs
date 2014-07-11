﻿using UnityEngine;
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
    private UILabel bestTimeLabel;
    private UILabel dropsFound;
    private UISprite lockedSprite;

    private Color32 collectibleFoundColor;
    private Color32 collectibleNotFoundColor;

    private Transition transition;

    private void Start()
    {
        button = GetComponent<UIButton>();
        title = transform.FindChild("Title").GetComponent<UILabel>();
        collectibleOne = transform.FindChild("Collectible 1").GetComponent<UISprite>();
        collectibleTwo = transform.FindChild("Collectible 2").GetComponent<UISprite>();
        collectibleThree = transform.FindChild("Collectible 3").GetComponent<UISprite>();
        loadedLevelLabel = transform.FindChild("Level Name").GetComponent<UILabel>();
        levelIndexLabel = transform.FindChild("Level Index").GetComponent<UILabel>();
        bestTimeLabel = transform.FindChild("Time").GetComponent<UILabel>();
        dropsFound = transform.FindChild("Drops").GetComponent<UILabel>();
        lockedSprite = transform.FindChild("Lock").GetComponent<UISprite>();
        collectibleNotFoundColor = new Color32(255, 255, 255, 100);
        collectibleFoundColor = new Color32(255, 255, 255, 255);

        transition = GameObject.Find("Transition").GetComponent<Transition>();

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
                lockedSprite.enabled = true;
                button.isEnabled = false;
            }
            else
            {
                locked = false;
                lockedSprite.enabled = false;
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

            if (SaveManager.CurrentSave.Levels[index].BestTime != 0)
                bestTimeLabel.text = SaveManager.CurrentSave.Levels[index].BestTime.ToString();
            else
                bestTimeLabel.text = "";

            if (SaveManager.CurrentSave.Levels[index].TotalDrops != 0)
            {
                dropsFound.text = SaveManager.CurrentSave.Levels[index].DropsCollected.ToString() + " / " +
                    SaveManager.CurrentSave.Levels[index].TotalDrops.ToString();
            }
            else
            {
                dropsFound.text = "";
            }
        }
    }

    private void OnClick()
    {
        if (!locked)
        {
            transition.PlayForward();
            transition.GetComponent<TweenScale>().AddOnFinished(new EventDelegate(this, "LoadLevel"));
        }
    }

    private void LoadLevel()
    {
        transition.GetComponent<TweenScale>().RemoveOnFinished(new EventDelegate(this, "LoadLevel"));
        Application.LoadLevel(loadedLevelLabel.text);
        transition.PlayReverse();
    }
}
