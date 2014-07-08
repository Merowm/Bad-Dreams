using UnityEngine;
using System.Collections;
using SaveSystem;

public class Collectible : MonoBehaviour
{
    //Types
    // "AddTime"
    // "AddPoints"
    // "AddTreasure"

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
    SoundHandler soundHandler;

    #endregion

    void Start()
    {
        soundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();

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
        soundHandler.PlaySound(SoundType.Time);
        ParticleSystem particleSystem = GameObject.Find("Clock Particle Effect").GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.transform.position = this.transform.position;
            particleSystem.renderer.sortingLayerName = "Pickups";
            particleSystem.Play();
        }

        Destroy(this.gameObject);
    }

    void AddPoints()
    {
        GameObject.Find("Drop Counter").GetComponent<DropCounter>().DropCount++;
        soundHandler.PlaySound(SoundType.Droplet);
		Instantiate(pickupParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    void AddTreasure()
    {
        soundHandler.PlaySound(SoundType.Treasure);
        Destroy(this.gameObject);
    }
}