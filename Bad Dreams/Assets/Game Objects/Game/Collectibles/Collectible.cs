using UnityEngine;
using System.Collections;
using SaveSystem;

public class Collectible : MonoBehaviour
{
    //Types
    // "AddTime"
    // "AddSeed"
    // "AddPoints"
    // "Treasure"

    #region Constants

    const float TIMER_MAX = 1.6f;
    const float TIMER_MIN = 1.2f;

    #endregion

    #region Public Variables

    public string type, pickupParticleResource;

    #endregion

    #region Private Variables

    bool goingUp;
    float min, max, speed, directionTimer, timerInterval;
    Vector3 origPos;
	GameObject pickupParticle;

    #endregion

    void Start()
    {
        min = 0.05f;
        max = -0.05f;
        directionTimer = 0.0f;
        timerInterval = Random.Range(TIMER_MIN, TIMER_MAX);
        speed = 1.0f / timerInterval;
        origPos = transform.position;

		pickupParticle = Resources.Load<GameObject>(pickupParticleResource);
    }

    void Update()
    {
        directionTimer += Time.deltaTime;

        if (goingUp)
        {
            transform.position = new Vector3(origPos.x, origPos.y + Mathf.SmoothStep(max, min, directionTimer * speed));
        }
        else
        {
            transform.position = new Vector3(origPos.x, origPos.y + Mathf.SmoothStep(min, max, directionTimer * speed));
        }

        if (directionTimer >= timerInterval)
        {
            directionTimer = 0.0f;
            goingUp = !goingUp;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Invoke(type, 0);
        }
    }

    void AddTime()
    {
        Debug.Log("+20 SECONDS");
        GameObject.Find("Timer").GetComponent<Timer>().TimeBonus();
        Destroy(this.gameObject);
    }

    void AddSeed()
    {
        Debug.Log("SEEEEEEEEEEEEEEEDS");
        GameObject.Find("Player").GetComponent<FlowerSkill>().Charges++;
        Destroy(this.gameObject);
    }

    void AddPoints()
    {
        GameObject.Find("Pick Up Droplets").GetComponent<AudioSource>().Play();
        Debug.Log("5 POINTS TO GRYFFINDOR (AKA We need a point system)");
		Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void AddTreasure()
    {
        GameObject.Find("Pick Up Treasure").GetComponent<AudioSource>().Play();
        Destroy(this.gameObject);
    }
}