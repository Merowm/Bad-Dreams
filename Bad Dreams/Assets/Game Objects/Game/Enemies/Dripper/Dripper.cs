using UnityEngine;
using System.Collections;

public class Dripper : MonoBehaviour 
{
    public float acidDelay;
    bool acidDelayOn;
    GameObject acidBall;

	// Use this for initialization
	void Start () 
    {
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
        GameObject ball = Instantiate(acidBall, transform.position, Quaternion.identity) as GameObject;
        ball.transform.parent = transform;
    }

    void AcidDelay()
    {
        acidDelayOn = false;
    }
}
