using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public string type;
    //types:
    // "Time"
    // "Flower"
    // "Points"

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Invoke(type, 0);
        }
    }

    void Time()
    {
        Debug.Log("+N SECONDS (AKA I guess we need a timer somewhere in the level)");
        Destroy(this.gameObject);
    }

    void Flower()
    {
        Debug.Log("SEEEEEEEEEEEEEEEDS (AKA Flower power needs a limit so that this will have any use)");
        Destroy(this.gameObject);
    }

    void Points()
    {
        Debug.Log("5 POINTS TO GRYFFINDOR (AKA We need a point system)");
        Destroy(this.gameObject);
    }
}