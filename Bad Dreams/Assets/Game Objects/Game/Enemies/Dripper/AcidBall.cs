using UnityEngine;
using System.Collections;

public class AcidBall : MonoBehaviour 
{
    const int LAYER_TERRAINCOLLISION = 8;
    const int LAYERMASK = 1 << 8;
    const float MAX_SPEED = 100.0f;

    float speed;
    bool speedCapped;
    float scale;

    bool falling;

    SpriteRenderer spriteRend;

	void Start () 
    {
        spriteRend = GetComponent<SpriteRenderer>();

        StartCoroutine(FadeToExistence(3.0f));
        name = "Acid Ball";
        speed = 0;
        falling = true;
	}
	
	void Update () 
    {
        if (falling)
        {
            if (!speedCapped)
            {
                speed += Time.deltaTime * 9.81f; //blasting off at the speed of sound
                if (speed >= MAX_SPEED)
                {
                    speedCapped = true;
                }
            }

            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);

            if (Physics2D.Raycast(transform.position, -Vector2.up, transform.localScale.x * 0.7f, LAYERMASK))
            {
                OnHitGround();
            }
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (col.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName == "Player Foreground")
            {
                OnHitPlayer();
            }
        }
    }

    void OnHitGround()
    {
        falling = false;
        Destroy(this.gameObject);
        //Destroy(this.collider2D);
        //AcidSplash();
    }

    void OnHitPlayer()
    {
        falling = false;
        Destroy(this.gameObject);
        GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
    }

    IEnumerator FadeToExistence(float timeMultiplier)
    {
        Color origColor = spriteRend.color;

        for (float t = 0.0f; t <= 1.2f; t += Time.deltaTime * timeMultiplier)
        {
            Color newColor = new Color(origColor.r, origColor.g, origColor.b, Mathf.Lerp(0, 1, t));
            spriteRend.color = newColor;
            yield return null;
        }
    }
}
