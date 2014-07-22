using UnityEngine;
using System.Collections;

public class BottomLimitCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            other.GetComponent<FallingAnimation>().ActivateAnimation();
        }
    }
}
