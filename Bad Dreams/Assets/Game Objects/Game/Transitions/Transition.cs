using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    private TweenScale tweenScale;

    public void Awake()
    {
        Transition[] gameObjects = GameObject.FindObjectsOfType<Transition>();

        if (gameObjects.Length > 1)
            DestroyImmediate(this.gameObject);
    }

    public void Start()
    {
        tweenScale = GetComponent<TweenScale>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        //if (!Application.isLoadingLevel && !tweenScale.enabled)
        //    PlayReverse();
    }

    public void PlayForward()
    {
        tweenScale.enabled = true;
        tweenScale.PlayForward();
    }

    public void PlayReverse()
    {
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
		}
		//hitA.DeleteDeathPrefab();
		//Debug.Log("onfin");
    }
}
