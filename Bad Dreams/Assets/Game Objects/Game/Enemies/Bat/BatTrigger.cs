using UnityEngine;
using System.Collections;

public class BatTrigger : MonoBehaviour 
{
    public bool ableToSwoop;
    BatAI batAI;

	void Start () 
    {
        ableToSwoop = true;
        batAI = transform.parent.gameObject.GetComponent<BatAI>();

        if (!batAI.debugging)
        {
            Destroy(GetComponent<SpriteRenderer>());
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (ableToSwoop)
            {
                batAI.Swoop();
                ableToSwoop = false;
            }
        }
    }
}
