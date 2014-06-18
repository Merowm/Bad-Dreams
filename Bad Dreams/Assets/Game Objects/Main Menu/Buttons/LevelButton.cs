using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    private UILabel label;

    private void Start()
    {
        label = GetComponentInChildren<UILabel>();
    }

    private void OnClick()
    {
        Application.LoadLevel(label.text);
    }
}
