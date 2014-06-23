using UnityEngine;
using System.Collections;

public class BatCollision : MonoBehaviour
{

	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            GameplayStateManager.SwitchTo(GameplayState.GameOver);
        }
    }
}
