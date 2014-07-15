using UnityEngine;
using System.Collections;

public class ButtonSelectedScale : MonoBehaviour
{
    private TweenScale tweenScale;

    private void Start()
    {
        tweenScale = GetComponent<TweenScale>();
    }

    public void Update()
    {
        if (UICamera.selectedObject != null && UICamera.hoveredObject != null)
            UICamera.selectedObject = null;

        if (UICamera.hoveredObject == this.gameObject)
        {
            UICamera.selectedObject = null;
            tweenScale.enabled = true;
        }
        if (UICamera.selectedObject == this.gameObject)
        {
            UICamera.hoveredObject = null;
            tweenScale.enabled = true;
        }
        

        if (UICamera.hoveredObject != this.gameObject &&
            UICamera.selectedObject != this.gameObject &&
            tweenScale.enabled)
        {
            if (tweenScale.direction == AnimationOrTween.Direction.Forward)
            {
                tweenScale.ResetToBeginning();
                tweenScale.enabled = false;
            }
        }
    }
}
