using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour 
{
    Transform point1, point2;
    GameObject bat;
    float x1, x2, y1, y2, finalA, finalB, finalC;
    float currentLocalX;
    bool flip, swooping;

    void Start()
    {
        point1 = transform.Find("Point 1");
        point2 = transform.Find("Point 2");
        bat = transform.Find("Batboy").gameObject;

        bat.transform.localPosition = point1.localPosition;
        currentLocalX = bat.transform.localPosition.x;

        CreateParabole();
    }

    void Update()
    {
        if (swooping)
        {
            if (flip)
                currentLocalX -= Time.deltaTime * 5.0f;
            else
                currentLocalX += Time.deltaTime * 5.0f;
            bat.transform.localPosition = new Vector3(currentLocalX, ReturnYCoordinate(currentLocalX), 0);

            if (currentLocalX >= x2)
            {
                Flip();
                swooping = false;
            }
            else if (currentLocalX <= x1)
            {
                Flip();
                swooping = false;
            }
        }
    }

    float ReturnYCoordinate(float xd)
    {
        float yd = (finalA * Mathf.Pow(xd, 2)) + (finalB * xd);
        return yd;
    }

    void CreateParabole()
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
        transform.Find("Batboy").GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-transform.Find("Batboy").GetComponent<SpriteRenderer>().transform.localScale.x,1,1);
    }

    public void Swoop()
    {
        swooping = true;
    }
}
