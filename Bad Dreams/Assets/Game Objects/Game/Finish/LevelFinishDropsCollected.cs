using UnityEngine;
using System.Collections;

public class LevelFinishDropsCollected : MonoBehaviour
{
    private UILabel uiLabel;

    private void OnEnable()
    {
        uiLabel = GetComponentInChildren<UILabel>();
        uiLabel.text = GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount.ToString() + " / " +
            GameObject.Find("LevelInfo").GetComponent<LevelInfo>().numberOfDrops;
    }
}
