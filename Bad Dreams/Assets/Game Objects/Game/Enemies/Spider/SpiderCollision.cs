using UnityEngine;
using System.Collections;

public class SpiderCollision : MonoBehaviour
{
    private SpiderAI spider;

    private void Start()
    {
        spider = GetComponent<SpiderAI>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
            GameplayStateManager.SwitchTo(GameplayState.GameOver);

        if (other.gameObject.name == "Spider Web")
            spider.SwitchTo(SpiderAIState.Idle);
    }
}
