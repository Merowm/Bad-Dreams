﻿using UnityEngine;
using System.Collections;

public class DogAI : MonoBehaviour
{
    #region Constants

    const float VISION_UPDATE_INTERVAL = 0.0f;
    const float TIME_VISION_RESET = 2.0f;
    const float TIME_SPENT_STILL_BEFORE_TURNING = 2.5f;
    const float TIME_ALERT_TIMER_MAX = 10.0f;
    const float COLLISION_DISTANCE_FROM_PLAYER_BEFORE_CAN_MOVE_AGAIN = 1.1f;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT = 4;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT_WHILE_ALERTED = 12;
    const int VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT = 1;
    const int VISION_BECOME_ALERTED_THRESHOLD = 5;
    const int ENEMY_LAYER = 9;
    const int SPEED_WALK = 1;
    const int SPEED_RUN = 2;
    const int SPEED_CHARGE = 5;
    const int BUMP_ALERTNESS_ADDITION = 6;

    #endregion

    #region Public Variables

    public bool Alerted
    {
        get
        {
            return alerted;
        }
    }

    #endregion

    #region Private Variables

    SpriteRenderer thisSpriteRend, thisVisionCone;
    Animator thisAnimator;
    BoxCollider2D thisCollider2D;
    Bounds thisBounds;
    Transform thisEyePos;

    GameObject player;
    SpriteRenderer playerSpriteRend;

    Vector3 currentDir;

    float playerDistance, viewLength, viewAngle, alertTimer, lastVisionCheckTimer, stoppedTimer, visionResetTimer, velocity;
    float alertness;
    bool playerVisible, alerted, stopped, visionAllowedToReset, flipAllowed, belowPlayer, recentlyCollidedWithPlayer, veryCloseToPlayer;

    AudioSource sound_DogSeePlayer, sound_DogCollision, sound_DogAlert;

    #endregion

    void Start()
    {
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_LAYER); //ignores collisions between enemies in the same layer
        thisCollider2D = GetComponent<BoxCollider2D>();
        viewLength = 6.0f; //every 1.0f equals to 64 pixels
        viewAngle = 45.0f; //the full view angle is double this, vision cone sprite should be aimed to the right
        player = GameObject.Find("Player");
        playerSpriteRend = player.transform.Find("Animator").GetComponent<SpriteRenderer>();
        thisSpriteRend = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        thisAnimator = transform.FindChild("Sprite").GetComponent<Animator>();
        currentDir = new Vector3(thisSpriteRend.transform.localScale.x, 0, 0);
        alertness = 0;
        alertTimer = 0.0f;
        playerVisible = false;
        alerted = false;
        stopped = false;
        lastVisionCheckTimer = 0.0f;
        stoppedTimer = 0.0f;
        visionResetTimer = 0.0f;
        visionAllowedToReset = true;
        velocity = 0.0f;
        thisEyePos = transform.FindChild("Eye Position");
        thisVisionCone = thisEyePos.FindChild("Vision Cone").gameObject.GetComponent<SpriteRenderer>();

        sound_DogSeePlayer = transform.Find("Dog Sounds/Dog See Player").GetComponent<AudioSource>();
        sound_DogAlert = transform.Find("Dog Sounds/Dog Alert").GetComponent<AudioSource>();
        sound_DogCollision = transform.Find("Dog Sounds/Dog Collision").GetComponent<AudioSource>();

    }

    void Update()
    {
        thisBounds = new Bounds(transform.position, thisCollider2D.size);

        GroundCheck();

        if (!visionAllowedToReset)
        {
            visionResetTimer += Time.deltaTime;
            UpdateIfAllowedToResetVision();
        }

        if (stopped)
        {
            if (playerVisible)
            {
                stoppedTimer = 0.0f; //if enemy sees player while enemy is on edge of its platform, reset stop timer
            }
            else
            {
                stoppedTimer += Time.deltaTime; //if it doesn't see player, keep adding to the timer
            }
        }

        else
        {
            if (alerted)
            {
                if (playerVisible)
                {
                    velocity = Mathf.Sign(currentDir.x) * SPEED_CHARGE;
                }
                else
                {
                    velocity = Mathf.Sign(currentDir.x) * SPEED_RUN;
                }
            }
            else
            {
                {
                    velocity = Mathf.Sign(currentDir.x) * SPEED_WALK;
                }
            }
        }

        lastVisionCheckTimer += Time.deltaTime;

        if (lastVisionCheckTimer >= VISION_UPDATE_INTERVAL)
        {
            ResetVisionTime();
            PlayerVisibilityCheck();
            CalculateVisionAngle();
            StopIfPlayerIsAbove();
            UpdateAlertness();
            UpdateVisionConeColor();
        }

        if (Vector3.Distance(player.transform.position, transform.position) > COLLISION_DISTANCE_FROM_PLAYER_BEFORE_CAN_MOVE_AGAIN)
        {
            veryCloseToPlayer = false;
        }

        if (stopped || recentlyCollidedWithPlayer || veryCloseToPlayer)
        {
            velocity = 0;
        }

        //animation
        if (velocity == 0)
        {
            CancelAnimations();
            thisAnimator.SetBool("stopped", true);
        }
        else if (Mathf.Abs(velocity) == 1)
        {
            CancelAnimations();
            thisAnimator.SetBool("walking", true);
        }
        else if (Mathf.Abs(velocity) > 1)
        {
            CancelAnimations();
            thisAnimator.SetBool("running", true);
        }

        //sounds
        if (alertness > 0.1f)
        {
            sound_DogSeePlayer.volume = alertness / VISION_BECOME_ALERTED_THRESHOLD;
        }
        else
        {
            sound_DogSeePlayer.volume = 0;
        }
        if (!veryCloseToPlayer)
        {
            if (alerted)
            {
                if (sound_DogAlert.volume < 1.0f)
                sound_DogAlert.volume += Time.deltaTime;
            }
            else
            {
                if (sound_DogAlert.volume > 0.0f)
                sound_DogAlert.volume -= Time.deltaTime;
            }
        }
        else
        {
            if (sound_DogAlert.volume > 0.0f)
            sound_DogAlert.volume -= Time.deltaTime;
        }

        thisSpriteRend.transform.localScale = new Vector3(Mathf.Sign(currentDir.x) * 0.2f, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);

        transform.position += new Vector3(velocity * Time.deltaTime, 0, 0);

        //alertness timer
        if (alerted)
        {
            if (!playerVisible)
            {
                if (alertTimer >= 0.0f)
                {
                    alertTimer -= Time.deltaTime;
                }
            }
        }

        DEBUG_KEYPRESSES();
        DEBUG_DRAWCURRENTDIR();
    }

    public bool ReturnIfPlayerInsideEnemyFOV()
    {
        if (ReturnAngleToPlayer() <= viewAngle)
        {
            return true;
        }
        return false;
    }

    void CancelAnimations()
    {
        thisAnimator.SetBool("stopped", false);
        thisAnimator.SetBool("walking", false);
        thisAnimator.SetBool("running", false);
    }

    void FlipAround()
    {
        thisSpriteRend.transform.localScale = new Vector3(-thisSpriteRend.transform.localScale.x, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);
        currentDir = new Vector3(thisSpriteRend.transform.localScale.x, 0, 0);
        thisEyePos.localPosition = new Vector3(Mathf.Abs(thisEyePos.localPosition.x) * Mathf.Sign(currentDir.x), thisEyePos.localPosition.y, thisEyePos.localPosition.z);

        if (playerVisible)
            recentlyCollidedWithPlayer = false;
    }

    void ChangePlayerSortingLayer()
    {
        if (playerSpriteRend.sortingLayerName == "Player Background")
        {
            Debug.Log("Player comes out from hiding");
            playerSpriteRend.sortingLayerName = "Player Foreground";
        }

        else
        {
            Debug.Log("Player hides");
            playerSpriteRend.sortingLayerName = "Player Background";
        }
    }

    void ToggleAlerted(bool value)
    {
        alerted = value;
    }

    void PlayerVisibilityCheck()
    {
        UpdateDistanceToPlayer();

        if (playerDistance < viewLength) //if player is in the enemy's vicinity
        {
            if (ReturnIfPlayerInsideEnemyFOV()) //if player is in the enemy's cone of vision
            {
                if (RayCastAtTarget(player)) //if the enemy has a clear line of sight at the player
                {

                    if (playerSpriteRend.sortingLayerName == "Player Background") //if player is in the hiding layer
                    {
                        if (playerVisible)
                        {
                            if (alerted)
                            {
                                AimEyesAtPlayer();
                                visionAllowedToReset = false;
                                visionResetTimer = 0.0f;
                                Debug.Log("Hidden in plain sight");
                                if (Physics2D.GetIgnoreLayerCollision(9, 10))
                                {
                                    Physics2D.IgnoreLayerCollision(9, 10, false);
                                }
                            }
                            else
                            {
                                playerVisible = false;
                                ResetVision();
                            }
                        }
                        else
                        {
                            DEBUG_DRAWVISIONLINETOPLAYER(Color.gray);
                        }
                    }
                    else //if player is in the visible layer
                    {
                        playerVisible = true;
                        AimEyesAtPlayer();
                        visionAllowedToReset = false;
                        visionResetTimer = 0.0f;
                    }
                }
                else
                {
                    playerVisible = false;
                    ResetVision();
                }
            }
            else if (playerVisible)
            {
                AimEyesAtPlayer();
            }
            else
            {
                playerVisible = false;
                ResetVision();
            }
        }
        else
        {
            playerVisible = false;
            ResetVision();
        }
    }

    void UpdateAlertness()
    {
        if (playerVisible)
        {
            if (alertness >= VISION_BECOME_ALERTED_THRESHOLD)
            {
                DEBUG_DRAWVISIONLINETOPLAYER(Color.red);
                alertTimer = TIME_ALERT_TIMER_MAX;
                if (!alerted)
                {
                    ToggleAlerted(true);
                    thisAnimator.SetBool("running", true);

                }
            }
            else
            {
                DEBUG_DRAWVISIONLINETOPLAYER(Color.yellow);
                alertness += VISION_ALERTNESS_ADDITION_ON_SIGHT * Time.deltaTime;
            }
        }
        else
        {
            if (alertness >= VISION_BECOME_ALERTED_THRESHOLD)
            {
                if (!alerted)
                {
                    alerted = true;
                }
            }

            if (alerted)
            {
                if (alertTimer <= 0.0f)
                {
                    alertness -= VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT * Time.deltaTime;

                    if (alertness <= 0)
                    {
                        alertness = 0;
                        ToggleAlerted(false);
                        thisAnimator.SetBool("running", false);
                    }
                }
            }
            else
            {
                if (alertness > 0)
                {
                    alertness -= VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT * Time.deltaTime;
                }
            }
        }
    }

    bool RayCastAtTarget(GameObject target)
    {
        //layermask just for testing
        int layerMask = 1 << 8;
        layerMask |= 1 << 10;
        RaycastHit2D rayCastResult = Physics2D.Raycast(thisEyePos.position, target.transform.position - thisEyePos.position, Vector3.Distance(target.transform.position, thisEyePos.position), layerMask);
        if (rayCastResult.collider == target.collider2D)
            return true;
        else return false;
    }

    void UpdateDistanceToPlayer()
    {
        playerDistance = Vector3.Distance(thisEyePos.position, player.transform.position);
    }

    float ReturnAngleToPlayer()
    {
        Vector2 angleToPlayer = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        float angle = Vector2.Angle(currentDir, angleToPlayer);
        return angle;
    }

    void ResetVisionTime()
    {
        lastVisionCheckTimer = 0.0f;
    }

    void DEBUG_KEYPRESSES()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            FlipAround();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            thisVisionCone.color = new Color(255, 0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            alerted = false;
            alertness = 0;
            alertTimer = 0.0f;
        }
    }

    void UpdateVisionConeColor()
    {
        if (alerted)
        {
            thisVisionCone.color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            if (playerVisible)
            {
                thisVisionCone.color = new Color(1, 1, 0, 0.3f);
            }
            else
            {
                thisVisionCone.color = new Color(1, 1, 1, 0.3f);
            }
        }
    }

    void DEBUG_DRAWVISIONLINETOPLAYER(Color color)
    {
        Debug.DrawRay(thisEyePos.position, player.transform.position - thisEyePos.position, color);
    }

    void DEBUG_DRAWCURRENTDIR()
    {
        Debug.DrawRay(thisEyePos.position, currentDir, Color.green);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (!playerVisible)
            {
                if (!alerted)
                {
                    if (!recentlyCollidedWithPlayer)
                    {
                        alertness += BUMP_ALERTNESS_ADDITION;
                        sound_DogCollision.Play();
                        Invoke("AimEyesAtPlayer", 0.8f);
                        Invoke("ResetCollisionWithPlayer", 2.0f);
                    }
                    recentlyCollidedWithPlayer = true;
                }
                else
                {
                    if (!recentlyCollidedWithPlayer)
                    {
                        GameObject.Find("Collision Against Dog Sound").GetComponent<AudioSource>().Play();
                        Invoke("AimEyesAtPlayer", 0.2f);
                        Invoke("ResetCollisionWithPlayer", 0.6f);
                    }
                    recentlyCollidedWithPlayer = true;
                }
            }
            else
            {
                veryCloseToPlayer = true;
				GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
                //GameplayStateManager.SwitchTo(GameplayState.GameOver);
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            if (playerVisible)
            {
                veryCloseToPlayer = true;
                GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
            }
            //GameplayStateManager.SwitchTo(GameplayState.GameOver);
        }
    }

    void ResetCollisionWithPlayer()
    {
        recentlyCollidedWithPlayer = false;
    }

    void FlipToFacePlayer()
    {
        if (!belowPlayer)
        {
            if (player.transform.position.x <= transform.position.x)
            {
                thisSpriteRend.transform.localScale = new Vector3(-0.2f, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);

                currentDir = new Vector3(-1, 0, 0);
            }
            else
            {
                thisSpriteRend.transform.localScale = new Vector3(0.2f, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);

                currentDir = new Vector3(1, 0, 0);
            }
            thisEyePos.localPosition = new Vector3(Mathf.Abs(thisEyePos.localPosition.x) * Mathf.Sign(currentDir.x), thisEyePos.localPosition.y, thisEyePos.localPosition.z);
        }
        else
        {
            velocity = 0;
        }
    }

    void AimEyesAtPlayer()
    {
        if (!belowPlayer)
        {
            currentDir = Vector3.Normalize(player.transform.position - thisEyePos.position);

            thisSpriteRend.transform.localScale = new Vector3(Mathf.Sign(currentDir.x) * 0.2f, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);

            thisEyePos.localPosition = new Vector3(Mathf.Abs(thisEyePos.localPosition.x) * Mathf.Sign(currentDir.x), thisEyePos.localPosition.y, thisEyePos.localPosition.z);
        }
        else
        {
            velocity = 0;
        }

        if (playerVisible)
        {
            recentlyCollidedWithPlayer = false;
        }
    }

    void ResetVision()
    {
        if (visionAllowedToReset)
        {
            currentDir = new Vector3(Mathf.Sign(currentDir.x) * 1, 0, 0);
        }
    }

    void GroundCheck()
    {
        Vector3 rayFrontDown = new Vector3(thisBounds.center.x + (Mathf.Sign(currentDir.x) * (thisBounds.max.x - thisBounds.center.x)), thisBounds.center.y, 0); //draw a ray from the lower front of the enemy
        Vector3 rayTopTowardsDirection = new Vector3(thisBounds.center.x, thisBounds.max.y, thisBounds.center.z);
        Vector3 rayBotTowardsDirection = new Vector3(thisBounds.center.x, thisBounds.min.y, thisBounds.center.z);

        Debug.DrawRay(rayTopTowardsDirection, new Vector3(Mathf.Sign(currentDir.x) * 0.7f, 0, 0));
        Debug.DrawRay(rayBotTowardsDirection, new Vector3(Mathf.Sign(currentDir.x) * 0.7f, 0, 0));
        Debug.DrawRay(rayFrontDown, new Vector3(0, -0.6f, 0));

        if (stopped)
        {
            UpdateIfStillStopped();
        }

        if (Raycast(rayBotTowardsDirection, new Vector3(Mathf.Sign(currentDir.x), 0, 0), 0.7f))
        {
            GroundCheckActions();
        }
        else if (Raycast(rayTopTowardsDirection, new Vector3(Mathf.Sign(currentDir.x), 0, 0), 0.7f))
        {
            GroundCheckActions();
        }

        else if (!Raycast(rayFrontDown, Vector3.down, 0.6f))
        {
            GroundCheckActions();
        }
        else
        {
            stopped = false;
        }
    }

    void UpdateIfStillStopped()
    {
        if (stoppedTimer >= TIME_SPENT_STILL_BEFORE_TURNING)
        {
            stoppedTimer = 0.0f;
            flipAllowed = true;
        }
    }

    void GroundCheckActions()
    {
        if (!stopped)
        {
            stoppedTimer = 0.0f;
            stopped = true;
            flipAllowed = false;
        }

        if (flipAllowed)
        {
            if (!playerVisible)
            {
                if (stopped)
                {
                    stopped = false;
                    FlipAround();
                }
            }
        }
    }

    bool Raycast(Vector3 pos, Vector3 direction, float length)
    {
        //LAYER 8 = TERRAIN
        //LAYER 9 = ENEMY
        RaycastHit2D hit = Physics2D.Raycast(pos, direction, length, 1 << 8);

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    void UpdateIfAllowedToResetVision()
    {
        if (visionResetTimer >= TIME_VISION_RESET)
        {
            visionResetTimer = 0.0f;
            visionAllowedToReset = true;
        }
    }

    void CalculateVisionAngle()
    {
        float angle = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;
        thisEyePos.rotation = Quaternion.Euler(0, 0, angle);
    }

    void StopIfPlayerIsAbove()
    {
        if (playerVisible)
        {
            float tempAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            if (tempAngle >= 60.0f && tempAngle <= 120.0f)
            {
                belowPlayer = true;
            }
            else belowPlayer = false;
        }
        else
        {
            belowPlayer = false;
        }
    }

    public void Reset()
    {
        alerted = false;
        alertness = 0;
        alertTimer = 0.0f;
    }
}