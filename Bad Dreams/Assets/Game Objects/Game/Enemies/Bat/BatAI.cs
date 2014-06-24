using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour 
{
    public bool debugging;

    Transform point1, point2;
    GameObject bat;
    Animator batAnim;
    public float batSpeed;
    float x1, x2, y1, y2, finalA, finalB, finalC;
    float currentLocalX;
    bool flip, swooping;

    BatTrigger batTrigger;

    bool Swooping
    {
        get { return swooping; }
        set 
        {
            swooping = value;
            if (swooping)
                batAnim.SetBool("swooping", true);
            else
                batAnim.SetBool("swooping", false);
        }
    }

    void Start()
    {
        point1 = transform.Find("Point 1");
        point2 = transform.Find("Point 2");

        if (!debugging)
        {
            Destroy(point1.gameObject.GetComponent<SpriteRenderer>());
            Destroy(point2.gameObject.GetComponent<SpriteRenderer>());
        }

        bat = transform.Find("Batboy").gameObject;
        batAnim = bat.GetComponent<Animator>();

        batTrigger = transform.Find("KEEP POINTS ABOVE THIS LINE").GetComponent<BatTrigger>();

        bat.transform.localPosition = point1.localPosition;
        currentLocalX = bat.transform.localPosition.x;

        CreateFlightParabole();
    }

    void Update()
    {
        if (Swooping)
        {
            if (flip)
                currentLocalX -= Time.deltaTime * batSpeed;
            else
                currentLocalX += Time.deltaTime * batSpeed;
            bat.transform.localPosition = new Vector3(currentLocalX, ReturnYCoordinate(currentLocalX), 0);

            if (!flip && currentLocalX > x2)
            {
                Flip();
                bat.transform.position = point2.position;
                Swooping = false;
                batTrigger.ableToSwoop = true;
            }
            else if (flip && currentLocalX < x1)
            {
                Flip();
                bat.transform.position = point1.position;
                Swooping = false;
                batTrigger.ableToSwoop = true;
            }
        }
    }

    float ReturnYCoordinate(float xd)
    {
        float yd = (finalA * Mathf.Pow(xd, 2)) + (finalB * xd);
        return yd;
    }

    void CreateFlightParabole()
    {
        x1 = point1.localPosition.x;
        x2 = point2.localPosition.x;
        y1 = point1.localPosition.y;
        y2 = point2.localPosition.y;

        finalA = ((y2) / (x2 * (x2 - x1))) - ((y1) / (x1 * (x2 - x1)));
        finalB = (y1 / x1) - finalA * x1;
    }

    void Flip()
    {
        flip = !flip;
        bat.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-bat.GetComponent<SpriteRenderer>().transform.localScale.x, 1, 1);
    }

    public void Swoop()
    {
        Swooping = true;
    }
}
