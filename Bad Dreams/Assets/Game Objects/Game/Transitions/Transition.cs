using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour
{
    private TweenScale tweenScale;

    public void Start()
    {
        tweenScale = GetComponent<TweenScale>();
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        if (!Application.isLoadingLevel && !tweenScale.enabled)
            PlayReverse();

        if (Input.GetKey(KeyCode.Return))
            PlayForward();
        if (Input.GetKey(KeyCode.Space))
            PlayReverse();
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
    }
}
