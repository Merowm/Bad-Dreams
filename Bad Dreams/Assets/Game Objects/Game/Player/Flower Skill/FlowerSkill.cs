using UnityEngine;
using System.Collections;

/// <summary>
/// Player skill that spawns 
/// a flower on the ground.
/// </summary>
public class FlowerSkill : MonoBehaviour
{
    public float skillDuration;

    private GameObject flower;
    private SpriteRenderer playerSpriteRenderer;
    private Player playerMovement;

    private void Start()
    {
        flower = Instantiate(Resources.Load("Flower")) as GameObject;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();// flower.GetComponent<SpriteRenderer>();
        flower.SetActive(false);
        playerMovement = GetComponent<Player>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Flower Skill") &&
            !flower.activeSelf &&
            playerMovement.onGround)
        {
            ActivateSkill();                
            Invoke("ResetSkill", skillDuration);
        }
    }

    private void ActivateSkill()
    {
        flower.SetActive(true);

        flower.transform.position = new Vector3(
            transform.position.x + playerSpriteRenderer.sprite.bounds.size.x,
            transform.position.y,
            transform.position.z);
    }

    private void ResetSkill()
    {
        flower.SetActive(false);
    }
}
