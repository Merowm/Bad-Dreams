using UnityEngine;
using System.Collections;
using SaveSystem;
using System.Collections.Generic;

public class LevelButton : MonoBehaviour
{
    private bool locked;

    private UIButton button;
    private UILabel title;
    private List<UISprite> collectibles;
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
        collectibles = new List<UISprite>();
        collectibles.Add(transform.FindChild("Collectible 1").GetComponent<UISprite>());
        collectibles.Add(transform.FindChild("Collectible 2").GetComponent<UISprite>());
        collectibles.Add(transform.FindChild("Collectible 3").GetComponent<UISprite>());
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

            // Locks
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

            // Collectibles
            for (int i = 0; i < SaveManager.CurrentSave.Levels[index].Collectibles.Count; ++i)
            {
                if (SaveManager.CurrentSave.Levels[index].Collectibles[i] == false)
                    collectibles[i].color = collectibleNotFoundColor;
                else
                    collectibles[i].color = collectibleFoundColor;
            }

            // Times
            if (SaveManager.CurrentSave.Levels[index].BestTime != 0)
            {
                int time = SaveManager.CurrentSave.Levels[index].BestTime;

                int minutes = (int)(time / 60);
                int seconds = (int)(time % 60);

                if (seconds < 10)
                {
                    bestTimeLabel.text = minutes + " : 0" + seconds;
                }
                else
                {
                    bestTimeLabel.text = minutes + " : " + seconds;
                }
            }
            else
            {
                bestTimeLabel.text = "";
            }

            // Drops
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
