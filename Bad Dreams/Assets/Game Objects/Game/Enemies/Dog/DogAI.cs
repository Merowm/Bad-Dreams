using UnityEngine;
using System.Collections;

public class DogAI : MonoBehaviour
{
    #region Constants

    const float VISION_UPDATE_INTERVAL = 0.0f;
    const float TIME_VISION_RESET = 2.0f;
    const float TIME_SPENT_STILL_BEFORE_TURNING = 2.5f;
    const float TIME_ALERT_TIMER_MAX = 10.0f;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT = 2;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT_WHILE_ALERTED = 4;
    const int VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT = 1;
    const int VISION_BECOME_ALERTED_THRESHOLD = 200;
    const int ENEMY_LAYER = 9;
    const int SPEED_WALK = 1;
    const int SPEED_RUN = 2;
    const int SPEED_CHARGE = 5;

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

    SpriteRenderer thisSpriteRend;
    Animator thisAnimator;
    BoxCollider2D thisCollider2D;
    Bounds thisBounds;

    GameObject player;
    SpriteRenderer playerSpriteRend;

    Vector3 currentDir;

    float playerDistance, viewLength, viewAngle, alertTimer, lastVisionCheckTimer, stoppedTimer, visionResetTimer, velocity;
    int alertness;
    bool playerVisible, alerted, stopped, visionAllowedToReset, flipAllowed;

    bool belowPlayer;

    Transform eyePos;

    #endregion

    void Start()
    {
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_LAYER); //ignores collisions between enemies in the same layer
        thisCollider2D = GetComponent<BoxCollider2D>();
        viewLength = 6.0f; //every 1.0f equals to 64 pixels
        viewAngle = 30.0f; //the full view angle is double this, vision cone sprite should be aimed to the right
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

        eyePos = transform.FindChild("Eye Position");
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
            DEBUG_UPDATECOLOR();
        }

        if (stopped)
        {
            velocity = 0;
        }

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
        eyePos.localPosition = new Vector3(Mathf.Abs(eyePos.localPosition.x) * Mathf.Sign(currentDir.x), eyePos.localPosition.y, eyePos.localPosition.z);
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
                if (alerted == false)
                {
                    ToggleAlerted(true);
                    thisAnimator.SetBool("running", true);

                }
            }
            else
            {
                DEBUG_DRAWVISIONLINETOPLAYER(Color.yellow);
                alertness += VISION_ALERTNESS_ADDITION_ON_SIGHT;
            }
        }
        else
        {
            if (alerted)
            {
                if (alertTimer <= 0.0f)
                {
                    alertness -= VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT;

                    if (alertness <= 0)
                    {
                        ToggleAlerted(false);
                        thisAnimator.SetBool("running", false);
                    }
                }
            }
            else
            {
                if (alertness > 0)
                {
                    alertness -= VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT;
                }
            }
        }
    }

    bool RayCastAtTarget(GameObject target)
    {
        //layermask just for testing
        int layerMask = 1 << 8;
        layerMask |= 1 << 10;
        RaycastHit2D rayCastResult = Physics2D.Raycast(eyePos.position, target.transform.position - eyePos.position, Vector3.Distance(target.transform.position, eyePos.position), layerMask);
        if (rayCastResult.collider == target.collider2D)
            return true;
        else return false;
    }

    void UpdateDistanceToPlayer()
    {
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
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

        /*if (Input.GetKeyDown(KeyCode.F))
        {
            ChangePlayerSortingLayer();
        }*/

        if (Input.GetKeyDown(KeyCode.C))
        {
            alerted = false;
            alertness = 0;
            alertTimer = 0.0f;
        }
    }

    void DEBUG_UPDATECOLOR()
    {
        if (alerted)
        {
            thisSpriteRend.color = new Color(255, 0, 0);
        }
        else
        {
            if (playerVisible)
            {
                thisSpriteRend.color = new Color(255, 255, 0);
            }
            else
            {
                thisSpriteRend.color = new Color(255, 255, 255);
            }
        }
    }

    void DEBUG_DRAWVISIONLINETOPLAYER(Color color)
    {
        Debug.DrawRay(eyePos.position, player.transform.position - eyePos.position, color);
    }

    void DEBUG_DRAWCURRENTDIR()
    {
        Debug.DrawRay(eyePos.position, currentDir, Color.green);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name + " hits the enemy collider");
        if (col.gameObject.name == "Player")
        {
            alertness += 20;
            if (alertness < 100)
            {
                alertness = 100;
            }
            else
            {
                FlipToFacePlayer();
                AimEyesAtPlayer();
            }
        }
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
            eyePos.localPosition = new Vector3(Mathf.Abs(eyePos.localPosition.x) * Mathf.Sign(currentDir.x), eyePos.localPosition.y, eyePos.localPosition.z);
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
            currentDir = Vector3.Normalize(player.transform.position - eyePos.position);

            thisSpriteRend.transform.localScale = new Vector3(Mathf.Sign(currentDir.x) * 0.2f, thisSpriteRend.transform.localScale.y, thisSpriteRend.transform.localScale.z);

            eyePos.localPosition = new Vector3(Mathf.Abs(eyePos.localPosition.x) * Mathf.Sign(currentDir.x), eyePos.localPosition.y, eyePos.localPosition.z);
        }
        else
        {
            velocity = 0;
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
            Debug.Log("Enemy is stopped");
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

        if (hit != null)
        {
            if (hit.collider != null)
            {
                return true;
            }
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
        eyePos.rotation = Quaternion.Euler(0, 0, angle);
    }

    void StopIfPlayerIsAbove()
    {
        if (playerVisible)
        {
            float tempAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            if (tempAngle >= 60.0f && tempAngle <= 120.0f)
            {
                Debug.Log("hap");
                belowPlayer = true;
            }
            else belowPlayer = false;
        }
        else
        {
            belowPlayer = false;
        }
    }
}