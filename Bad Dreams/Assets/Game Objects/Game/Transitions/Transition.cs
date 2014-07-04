using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    public Texture defaultTransition;
    public Texture deathTransition;
    public Texture levelFinishTransition;

    private TweenScale tweenScale;
    private GUITexture guiTexture;

    public void Awake()
    {
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

    public void PlayForward(TransitionStyle transitionStyle = TransitionStyle.Default)
    {
        DetermineTransitionTexture(transitionStyle);
        tweenScale.enabled = true;
        tweenScale.PlayForward();
    }

    public void PlayReverse(TransitionStyle transitionStyle = TransitionStyle.Default)
    {
        DetermineTransitionTexture(transitionStyle);
        tweenScale.enabled = true;
        tweenScale.PlayReverse();
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
		//hitA.DeleteDeathPrefab();
		//Debug.Log("onfin");
    }

    public void DetermineTransitionTexture(TransitionStyle transitionStyle)
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
}
