using UnityEngine;
using System.Collections;

public class ButtonFade : MonoBehaviour
{
    private UISprite uiSprite;
    private bool fadeIn;
    private float fadeSpeed;

    private void Start()
    {
        uiSprite = GetComponent<UISprite>();
        fadeSpeed = 0.1F;
    }

    private void Update()
    {
        if (fadeIn)
        {
            if (uiSprite.color.a < 1)
            {
                uiSprite.color = new Color(
                    uiSprite.color.r,
                    uiSprite.color.g,
                    uiSprite.color.b,
                    uiSprite.color.a + fadeSpeed);
            }
            else
            {
                fadeIn = false;
            }
        }
    }

    private void OnEnable()
    {
        fadeIn = true;
    }

    private void OnDisable()
    {
        if (uiSprite != null)
        {
            uiSprite.color = new Color(
                uiSprite.color.r,
                uiSprite.color.g,
                uiSprite.color.b,
                0);
        }
    }
}
