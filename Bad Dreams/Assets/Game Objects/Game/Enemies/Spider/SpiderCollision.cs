using UnityEngine;
using System.Collections;

public class SpiderCollision : MonoBehaviour
{
    private SpiderAI spider;
    SoundHandler soundHandler;

    private void Start()
    {
        spider = GetComponent<SpiderAI>();
        soundHandler = GameObject.Find("Sound Handler").GetComponent<SoundHandler>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            soundHandler.PlaySound(SoundType.SpiderRattle);
            GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
        }

        if (other.gameObject.name == "Spider Web")
            spider.SwitchTo(SpiderAIState.Idle);
    }
}
