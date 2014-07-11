using UnityEngine;
using System.Collections;

public class Dripper : MonoBehaviour 
{
    public float acidDelay;
    bool acidDelayOn;
    GameObject acidBall;
    Transform tip;

	// Use this for initialization
	void Start () 
    {
        tip = transform.Find("Tip");
        acidBall = Resources.Load("Enemies/Acidball") as GameObject;
        acidDelayOn = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!acidDelayOn)
        {
            DropAcid();
            acidDelayOn = true;
            Invoke("AcidDelay", acidDelay);
        }
	}

    void DropAcid()
    {
        GameObject ball = Instantiate(acidBall, tip.position, Quaternion.identity) as GameObject;
        ball.transform.parent = transform;
    }

    void AcidDelay()
    {
        acidDelayOn = false;
    }
}
