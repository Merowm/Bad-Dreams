using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!player.onGround)
            transform.parent = null;
    }

    // Change to use tags such as "Enemy" & "Moving Platform"
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Dog")
        {
            GameplayStateManager.SwitchTo(GameplayState.GameOver);
        }

        if (other.gameObject.name == "plat" && player.onGround)
            transform.parent = other.gameObject.transform;
    }
}
