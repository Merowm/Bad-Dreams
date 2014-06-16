using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Dog")
        {
            GameplayStateManager.SwitchTo(GameplayState.GameOver);
        }
    }
}
