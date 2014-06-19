using UnityEngine;
using System.Collections;

public class BatTrigger : MonoBehaviour {

    BatAI batAI;

	void Start () 
    {
        batAI = transform.parent.gameObject.GetComponent<BatAI>();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            batAI.Swoop();
        }
    }
}
