using UnityEngine;
using System.Collections;

public class LevelFinishTime : MonoBehaviour
{
    private UILabel uiLabel;

    private void OnEnable()
    {
        uiLabel = GetComponentInChildren<UILabel>();
        uiLabel.text = GameObject.Find("Timer").GetComponent<Timer>().TimePassed.ToString();
    }
}
