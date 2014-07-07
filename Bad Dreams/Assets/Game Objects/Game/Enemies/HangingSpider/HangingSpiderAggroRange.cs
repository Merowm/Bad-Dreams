using UnityEngine;
using System.Collections;

public class HangingSpiderAggroRange : MonoBehaviour
{
    private HangingSpider spider;

    private void Start()
    {
        spider = transform.parent.GetComponent<HangingSpider>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (spider.State == HangingSpiderState.Idle)
            {
                spider.SwitchTo(HangingSpiderState.Descending);
            }
        }
    }
}
