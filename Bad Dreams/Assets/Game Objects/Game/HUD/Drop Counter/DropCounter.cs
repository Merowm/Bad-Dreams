using UnityEngine;
using System.Collections;

public class DropCounter : MonoBehaviour
{
    private int dropCount;
    private UILabel uiLabel;

    private void Start()
    {
        dropCount = 0;
        uiLabel = GameObject.Find("Drop Counter Text").GetComponentInChildren<UILabel>();
    }

    public int DropCount
    {
        get { return dropCount; }
        set
        {
            dropCount = Mathf.Clamp(value, 0, int.MaxValue);
            uiLabel.text = dropCount.ToString();
        }
    }
}
