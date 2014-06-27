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
    private Player player;
    private HidingSkill hidingSkill;
    private BoxCollider2D flowerCollider;
    private bool skillUsable;
    private int _Charges;

    private void Start()
    {
        flower = Instantiate(Resources.Load("Player/Flower")) as GameObject;
        
        flower.SetActive(false);
        player = GetComponent<Player>();
        hidingSkill = GetComponent<HidingSkill>();
        flowerCollider = GetComponent<BoxCollider2D>();
        skillUsable = true;
        Charges = 3;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Flower Skill") &&
            skillUsable &&
            player.onGround &&
            !hidingSkill.IsHiding &&
            !hidingSkill.OverCoverObject &&
            Charges > 0)
        {
            ActivateSkill();
            Invoke("StopSkill", skillDuration);    
        }
    }

    private void ActivateSkill()
    {
        skillUsable = false;
        --Charges;
        flower.SetActive(true);
        flower.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z);

        hidingSkill.Hide(flower);

		if (player.parentObject != null)
		{
			flower.GetComponent<Flower>().parentObject = player.parentObject;
			flower.GetComponent<Flower>().offsetFromPlatform = transform.position - player.parentObject.transform.position;
			//flower.transform.parent = player.transform.parent;
		}
		else
		{
			flower.GetComponent<Flower>().parentObject = null;
			//flower.transform.parent = null;
		}
    }

    public void StopSkill()
    {
        if (hidingSkill.IsHiding &&
            hidingSkill.CoverObject.tag == "Flower Skill Cover")
        {
            hidingSkill.Unhide();
        }

		hidingSkill.OverCoverObject = false;
		hidingSkill.HidingPossible = false;
        flower.SetActive(false);
        skillUsable = true;
    }

    public int Charges
    {
        get { return _Charges; }
        set
        {
            _Charges = Mathf.Clamp(value, 0, 3);
        }
    }
}
