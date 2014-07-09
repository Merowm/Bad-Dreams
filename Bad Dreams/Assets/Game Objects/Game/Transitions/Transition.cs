using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    public Texture defaultTransition;
    public Texture deathTransition;
    public Texture levelFinishTransition;
    private TweenScale tweenScale;
    private GUITexture guiTexture;
    private State state;

    private enum State
    {
        Disabled,
        Forward,
        Reverse
    }

    public void Awake()
    {
        state = State.Disabled;
        Transition[] gameObjects = GameObject.FindObjectsOfType<Transition>();

        if (gameObjects.Length > 1)
            DestroyImmediate(this.gameObject);
    }

    public void Start()
    {
        tweenScale = GetComponent<TweenScale>();
        guiTexture = GetComponent<GUITexture>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        switch (state)
        {
            case State.Disabled:
                break;

            case State.Forward:
                IncreaseAlpha();
                break;

            case State.Reverse:
                DecreaseAlpha();
                break;
        }
    }

    public void PlayForward(TransitionStyle transitionStyle = TransitionStyle.Default)
    {
        DetermineTransitionTexture(transitionStyle);
        tweenScale.enabled = true;
        tweenScale.PlayForward();
        state = State.Forward;
    }

    public void PlayReverse(TransitionStyle transitionStyle = TransitionStyle.Default)
    {
        DetermineTransitionTexture(transitionStyle);
        tweenScale.enabled = true;
        tweenScale.PlayReverse();
        state = State.Reverse;
    }

    public void OnFinished()
    {
        tweenScale.enabled = false;
		GameObject playerObj = GameObject.Find("Player");
		if (playerObj)
		{
			HitAnimation hitA = playerObj.GetComponent<HitAnimation>();
			hitA.ResetAnimation();

			FallingAnimation fallA = playerObj.GetComponent<FallingAnimation>();
			fallA.ResetAnimation();
		}
    }

    private void DetermineTransitionTexture(TransitionStyle transitionStyle)
    {
        switch (transitionStyle)
        {
            case TransitionStyle.Default:
                guiTexture.texture = defaultTransition;
                break;

            case TransitionStyle.Death:
                guiTexture.texture = deathTransition;
                break;

            case TransitionStyle.LevelFinish:
                guiTexture.texture = levelFinishTransition;
                break;
        }
    }

    private float alphaIncrease = 0.0125F;
    private float alphaDecrease = 0.02F;

    private void IncreaseAlpha()
    {
        if (guiTexture.color.a < 255)
        {
            guiTexture.color = new Color(
                guiTexture.color.r,
                guiTexture.color.g,
                guiTexture.color.b,
                guiTexture.color.a + alphaIncrease);
        }
        else
        {
            state = State.Disabled;
        }
    }

    private void DecreaseAlpha()
    {
        if (guiTexture.color.a > 0)
        {
            guiTexture.color = new Color(
                guiTexture.color.r,
                guiTexture.color.g,
                guiTexture.color.b,
                guiTexture.color.a - alphaDecrease);
        }
        else
        {
            state = State.Disabled;
        }
    }
}
