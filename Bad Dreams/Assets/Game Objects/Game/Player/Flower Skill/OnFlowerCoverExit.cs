using UnityEngine;
using System.Collections;

public class OnFlowerCoverExit : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            FlowerSkill flowerSkill = other.GetComponent<FlowerSkill>();
            flowerSkill.StopSkill();
        }
    }
}
