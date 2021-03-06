﻿using UnityEngine;
using System.Collections;

public class SpiderWebCollision : MonoBehaviour 
{
    private SpiderAI spider;

    private void Start()
    {
        spider = GetComponentInChildren<SpiderAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            spider.SwitchTo(SpiderAIState.Chasing);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            spider.SwitchTo(SpiderAIState.Idle);
        }
    }
}
