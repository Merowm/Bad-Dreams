using UnityEngine;
using System.Collections;

public class SpiderWeb : MonoBehaviour 
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Spider")
        {
            SpiderAI spiderAI = other.GetComponent<SpiderAI>();
            spiderAI.State = SpiderAIState.Idle;
        }
    }
}
