using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowerCharges : MonoBehaviour
{
    private FlowerSkill flowerSkill;
    private List<GameObject> charges;

    private void Start()
    {
        flowerSkill = GameObject.Find("Player").GetComponent<FlowerSkill>();
        charges = new List<GameObject>();
        foreach (Transform child in transform)
            charges.Add(child.gameObject);
    }

    private void Update()
    {
        switch (flowerSkill.Charges)
        {
            case 0:
                SetObjectsActive(charges, 0);
                break;

            case 1:
                SetObjectsActive(charges, 1);
                break;

            case 2:
                SetObjectsActive(charges, 2);
                break;

            case 3:
                SetObjectsActive(charges, 3);
                break;
        }
    }

    private void SetObjectsActive(List<GameObject> objects, int count)
    {
        if (count <= objects.Count)
        {
            for (int i = 0; i < objects.Count; ++i)
                objects[i].SetActive(false);

            for (int i = 0; i < count; i++)
                objects[i].SetActive(true);
        }
    }
}
