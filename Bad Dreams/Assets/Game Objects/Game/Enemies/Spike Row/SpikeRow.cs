using UnityEngine;
using System.Collections;

public class SpikeRow : MonoBehaviour 
{
    public int spikeAmount;
    public float spikeDelay;
    public float spikeEndDelay;

    float leftSideX, rightSideX;
    Vector2[] vectors;
    int currentSpike;
    GameObject spike;
    bool activated;

	void Start () 
    {
        activated = true;
        spike = Resources.Load("Enemies/Spike") as GameObject;
        leftSideX = transform.Find("Left Side").position.x;
        rightSideX = transform.Find("Right Side").position.x;

        currentSpike = 0;

        float maxLength = (rightSideX - leftSideX) / (spikeAmount - 1);

        vectors = new Vector2[spikeAmount];

        for (int i = 0; i < spikeAmount; i++)
        {
            vectors[i] = new Vector2(leftSideX + maxLength * i, transform.position.y);
        }

        MakeSpike();
	}

    void MakeSpike()
    {
        if (currentSpike >= spikeAmount)
        {
            currentSpike = 0;
            Invoke("MakeSpike", spikeEndDelay);
            return;
        }

        Instantiate(spike, vectors[currentSpike], Quaternion.identity);
        ++currentSpike;

        if (activated)
        Invoke("MakeSpike", spikeDelay);
    }

    void OnEnable()
    {
        activated = true;
        currentSpike = 0;
        Invoke("MakeSpike", spikeDelay);
    }

    void OnDisable()
    {
        activated = false;
    }
}
