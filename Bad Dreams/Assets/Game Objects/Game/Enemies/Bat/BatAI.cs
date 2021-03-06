﻿using UnityEngine;
using System.Collections;

public class BatAI : MonoBehaviour
{
    #region Constants

    const float BAT_Y_VARIATION = 0.2f;
    const float BAT_HIT_AREA = 0.6f;

    #endregion

    #region Public Variables

    public bool debugging, alwaysSwoop;
    public float batSpeed, alwaysSwoopDelay;

    #endregion

    #region Private Variables

    Transform point1, point2;
    GameObject bat, player;
    Animator batAnim;
    BatSounds batSounds;

    float x1, x2, y1, y2, finalA, finalB, currentLocalX, alwaysSwoopTimer, currentBatSpeed;
    bool flip, swooping;

    bool Swooping
    {
        get { return swooping; }
        set
        {
            if (swooping != value)
            {
                swooping = value;
                if (swooping)
                    batAnim.SetBool("swooping", true);
                else
                    batAnim.SetBool("swooping", false);
            }
        }
    }

    #endregion

    void Start()
    {
        currentBatSpeed = 0;
        alwaysSwoopTimer = 0.0f;

        player = GameObject.Find("Player");

        point1 = transform.Find("Point 1");
        point2 = transform.Find("Point 2");

        CreateFlightParabola();

        if (!debugging)
        {
            Destroy(point1.gameObject.GetComponent<SpriteRenderer>());
            Destroy(point2.gameObject.GetComponent<SpriteRenderer>());
        }
        Destroy(transform.Find("KEEP POINTS ABOVE THIS LINE").gameObject);

        bat = transform.Find("Batboy").gameObject;
        batAnim = bat.GetComponent<Animator>();

        bat.transform.localPosition = point1.localPosition;
        currentLocalX = bat.transform.localPosition.x;

        batSounds = bat.GetComponent<BatSounds>();

    }

    void Update()
    {
        if (!swooping)
        {
            if (currentBatSpeed != 0)
            {
                currentBatSpeed = 0;
                Flip();
            }
        }

        if (alwaysSwoop)
        {
            if (!swooping)
            {
                alwaysSwoopTimer += Time.deltaTime;
                if (alwaysSwoopTimer >= alwaysSwoopDelay)
                {
                    SwoopingDelay();
                }
            }
        }
        else
            if (!swooping)
            {
                CheckIfPlayerCrossesOverFlightPath();
            }
        


        if (Swooping)
        {
            UpdateBatPosition();
            CheckIfPlayerIsWithinAttackReach();
            FlightEndActions();
        }
    }

    void UpdateBatPosition()
    {
        if (flip)
            currentLocalX -= Time.deltaTime * currentBatSpeed;
        else
            currentLocalX += Time.deltaTime * currentBatSpeed;
        bat.transform.localPosition = new Vector3(currentLocalX, ReturnYCoordinateOnParabola(currentLocalX), 0);
    }

    void CheckIfPlayerIsWithinAttackReach()
    {
        if (Vector3.Distance(bat.transform.position, player.transform.position) < BAT_HIT_AREA)
        {
            player.GetComponent<HitAnimation>().ActivateAnimation();
        }
    }

    void FlightEndActions()
    {
        if (!flip && currentLocalX > x2)
        {
            Swooping = false;
            //Flip();
            bat.transform.position = point2.position;
        }
        else if (flip && currentLocalX < x1)
        {
            Swooping = false;
            //Flip();
            bat.transform.position = point1.position;
        }
    }

    float ReturnYCoordinateOnParabola(float xd)
    {
        float yd = (finalA * Mathf.Pow(xd, 2)) + (finalB * xd);
        return yd;
    }

    void CreateFlightParabola()
    {
        x1 = point1.localPosition.x;
        x2 = point2.localPosition.x;
        y1 = point1.localPosition.y;
        y2 = point2.localPosition.y;

        finalA = ((y2) / (x2 * (x2 - x1))) - ((y1) / (x1 * (x2 - x1)));
        finalB = (y1 / x1) - finalA * x1;
    }

    void Flip()
    {
        flip = !flip;
        bat.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(-bat.GetComponent<SpriteRenderer>().transform.localScale.x, 1, 1);
    }

    void CheckIfPlayerCrossesOverFlightPath()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        if (playerPos.x >= x1 && playerPos.x <= x2)
        {
            if (Mathf.Abs(playerPos.y - ReturnYCoordinateOnParabola(playerPos.x)) <= BAT_Y_VARIATION)
            {
                if (swooping == false)
                    batSounds.BatSqueak();

                Swooping = true;
                currentBatSpeed = batSpeed;
            }
        }
    }

    void SwoopingDelay()
    {
        Swooping = true;
        alwaysSwoopTimer = 0.0f;
    }
}
