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
            GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();

        if (other.gameObject.name == "Spider Web")
            spider.SwitchTo(SpiderAIState.Idle);
    }
}
