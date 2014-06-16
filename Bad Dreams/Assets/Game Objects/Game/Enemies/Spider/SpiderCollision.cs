using UnityEngine;
using System.Collections;

public class SpiderCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameplayStateManager.SwitchTo(GameplayState.GameOver);
        }
    }
}
