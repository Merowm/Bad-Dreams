using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour 
{
    const int LAYERMASK = 1 << 8;
    const float MAX_SPEED = 100.0f;

    SpriteRenderer spriteRend;
    bool ready;
    float speed;
    bool speedCapped;

	void Start () 
    {
        spriteRend = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeToExistence(2.0f));
	}
	
	void Update () 
    {
        if (ready)
        {
            if (!speedCapped)
            {
                speed += Time.deltaTime * 9.81f;
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
        Destroy(this.gameObject);
    }

    void OnHitPlayer()
    {
        Destroy(this.gameObject);
        GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
    }

    IEnumerator FadeToExistence(float timeDivider)
    {
        Color origColor = spriteRend.color;

        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / timeDivider)
        {
            Color newColor = new Color(origColor.r, origColor.g, origColor.b, Mathf.Lerp(0, 1, t));
            spriteRend.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        ready = true;
    }
}
