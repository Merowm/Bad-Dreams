﻿using UnityEngine;
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
    private bool skillUsable;
    private int _Charges;

    private void Start()
    {
        flower = Instantiate(Resources.Load("Player/Flower")) as GameObject;
        
        flower.SetActive(false);
        player = GetComponent<Player>();
        hidingSkill = GetComponent<HidingSkill>();
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
            Invoke("CooldownFinished", skillDuration);
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

        if (player.transform.parent != null)
            flower.transform.parent = player.transform.parent;
        else
            flower.transform.parent = null;
    }

    public void StopSkill()
    {
        if (hidingSkill.IsHiding &&
            hidingSkill.CoverObject.name == "Flower(Clone)")
        {
            hidingSkill.Unhide();
            hidingSkill.HidingPossible = false;
        }

        flower.SetActive(false);
    }

    private void CooldownFinished()
    {
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
