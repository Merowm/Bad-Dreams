using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour 
{
    Transform point1, point2;
    GameObject bat;
    float x1, x2, y1, y2, finalA, finalB, finalC;
    float currentLocalX;
    bool flip;

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
        Debug.Log("Fingers crossed, final A " + finalA);
        Debug.Log("Fingers crossed, final B " + finalB);

        if (flip)
            currentLocalX -= Time.deltaTime * 5.0f;
        else
            currentLocalX += Time.deltaTime * 5.0f;

        bat.transform.localPosition = new Vector3(currentLocalX, ReturnYCoordinate(currentLocalX), 0);

        if (currentLocalX >= x2)
        {
            flip = true;
        }
        else if (currentLocalX <= x1)
        {
            flip = false;
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

    void ToggleFlip()
    {
        flip = !flip;
    }
}
