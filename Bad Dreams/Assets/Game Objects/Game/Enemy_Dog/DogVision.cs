using UnityEngine;
using System.Collections;

public class DogVision : MonoBehaviour
{
    const float VISION_UPDATE_INTERVAL = 0.0f;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT = 2;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT_WHILE_ALERTED = 4;
    const int VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT = 1;
    const int VISION_BECOME_ALERTED_LIMIT = 200;
    const float VISION_RESET_TIME = 2.0f;
    const int ENEMY_LAYER = 9;
    const int HIDDEN_LAYER = 10;
    const int SPEED_WALK = 1;
    const int SPEED_RUN = 2;
    const int SPEED_CHARGE = 5;
    const float TIME_SPENT_STILL_BEFORE_TURNING = 2.5f;

    GameObject player;
    SpriteRenderer spriteRend, playerSpriteRend;
    Vector3 currentDir;
    float playerDistance, viewLength, viewAngle, alertTimer, lastVisionCheckTimer, stoppedTimer, visionResetTimer;
    int alertness;
    bool playerVisible, alerted, stopped, stoppedBefore, visionAllowedToReset;

    void Start()
    {
        viewLength = 6.0f;
        viewAngle = 35.0f;
        player = GameObject.Find("Player");
        playerSpriteRend = player.transform.Find("Animator").GetComponent<SpriteRenderer>();
        spriteRend = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
        currentDir = new Vector3(spriteRend.transform.localScale.x, 0, 0);
        alertness = 0;
        alertTimer = 0.0f;
        playerVisible = false;
        alerted = false;
        stopped = false;
        stoppedBefore = false;
        lastVisionCheckTimer = 0.0f;
        stoppedTimer = 0.0f;
        visionResetTimer = 0.0f;
        visionAllowedToReset = true;
    }

    void Update()
    {
        //dogmovement
        GroundCheck();

        if (!visionAllowedToReset)
        {
            visionResetTimer += Time.deltaTime;
            UpdateIfAllowedToResetVision();
        }

        if (stopped)
        {
            if (!playerVisible)
            {
                stoppedTimer += Time.deltaTime;
            }
            else
            {
                //stopped = false;
                stoppedTimer = 0.0f;
            }
        }

        else
        {
            if (alerted)
            {
                if (playerVisible)
                {
                    if (!stopped)
                    transform.position += new Vector3(Mathf.Sign(currentDir.x) * SPEED_CHARGE * Time.deltaTime, 0, 0);
                }
                else
                {
                    if (!stopped)
                    transform.position += new Vector3(Mathf.Sign(currentDir.x) * SPEED_RUN * Time.deltaTime, 0, 0);
                }
            }
            else
            {
                if (playerVisible)
                {
                }
                else
                {
                    if (!stopped)
                    transform.position += new Vector3(Mathf.Sign(currentDir.x) * SPEED_WALK * Time.deltaTime, 0, 0);
                }
            }
        }

        



        //vision
        lastVisionCheckTimer += Time.deltaTime;

        if (lastVisionCheckTimer >= VISION_UPDATE_INTERVAL)
        {
            ResetVisionTime();
            PlayerVisibilityCheck();
            UpdateAlertness();
            DEBUG_UPDATECOLOR();
        }

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

    void FlipAround()
    {
        spriteRend.transform.localScale = new Vector3(-spriteRend.transform.localScale.x, 1, 1);

        currentDir = new Vector3(spriteRend.transform.localScale.x, 0, 0);
    }

    void FlipAround(int dir)
    {
        spriteRend.transform.localScale = new Vector3(dir, 1, 1);

        currentDir = new Vector3(dir, 0, 0);
    }

    void ChangePlayerLayer()
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
            if (ReturnAngleToPlayer() <= viewAngle) //if player is in the enemy's cone of vision
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
                                Debug.Log("Hidden in plain sight");
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
                        //add check for if the player went into hiding in plain sight
                    }
                    else //if player is in the visible layer
                    {
                        playerVisible = true;
                        AimEyesAtPlayer();
                        visionAllowedToReset = false;
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
            if (alertness >= VISION_BECOME_ALERTED_LIMIT)
            {
                DEBUG_DRAWVISIONLINETOPLAYER(Color.red);
                alertTimer = 10.0f;
                if (alerted == false)
                {
                    ToggleAlerted(true);
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
        int layerMask = 1 << ENEMY_LAYER;
        layerMask = ~layerMask;
        RaycastHit2D rayCastResult = Physics2D.Raycast(transform.position, target.transform.position - transform.position, Vector3.Distance(target.transform.position, transform.position), layerMask);
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
        float angle = Vector3.Angle(currentDir, player.transform.position - transform.position);
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
            ChangePlayerLayer();
        }
    }

    void DEBUG_UPDATECOLOR()
    {
        if (alerted)
        {
            spriteRend.color = new Color(255, 0, 0);
        }
        else
        {
            if (playerVisible)
            {
                spriteRend.color = new Color(255, 255, 0);
            }
            else
            {
                spriteRend.color = new Color(255, 255, 255);
            }
        }
    }

    void DEBUG_DRAWVISIONLINETOPLAYER(Color color)
    {
        Debug.DrawRay(transform.position, player.transform.position - transform.position, color);
    }

    void DEBUG_DRAWCURRENTDIR()
    {
        Debug.DrawRay(transform.position, currentDir, Color.green);
    }

    //requires player to have a RigidBody2D
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name + " hits the enemy collider");
        if (col.gameObject.name == "Player")
        {
            if (alertness < 100)
            {
                alertness = 100;
            }
            FlipToFacePlayer();
        }
    }

    void FlipToFacePlayer()
    {
        if (player.transform.position.x <= transform.position.x)
        {
            spriteRend.transform.localScale = new Vector3(-1, 1, 1);

            currentDir = new Vector3(-1, 0, 0);
        }
        else
        {
            spriteRend.transform.localScale = new Vector3(1, 1, 1);

            currentDir = new Vector3(1, 0, 0);
        }
    }

    void AimEyesAtPlayer()
    {
        spriteRend.transform.localScale = new Vector3(Mathf.Sign(currentDir.x) * 1, 1, 1);

        currentDir = Vector3.Normalize(player.transform.position - transform.position);
    }

    void ResetVision()
    {
        if (visionAllowedToReset)
        {
            currentDir = new Vector3(Mathf.Sign(currentDir.x) * 1, 0, 0);
        }
    }

    //movement methods

    void GroundCheck()
    {
        Vector3 rayLeftDown = new Vector3(spriteRend.bounds.min.x, transform.position.y, transform.position.z);
        Vector3 rayRightDown = new Vector3(spriteRend.bounds.max.x, transform.position.y, transform.position.z);
        Vector3 rayTopTowardsDirection = new Vector3(transform.position.x, spriteRend.bounds.max.y, transform.position.z);
        Vector3 rayBotTowardsDirection = new Vector3(transform.position.x, spriteRend.bounds.min.y, transform.position.z);

        Debug.DrawRay(rayTopTowardsDirection, new Vector3(Mathf.Sign(currentDir.x) * 0.5f, 0, 0));
        Debug.DrawRay(rayBotTowardsDirection, new Vector3(Mathf.Sign(currentDir.x) * 0.5f, 0, 0));
        Debug.DrawRay(rayRightDown, Vector3.down);
        Debug.DrawRay(rayLeftDown, Vector3.down);

        if (stopped)
        {
            UpdateIfStillStopped();
        }

        if (!stopped)
        {
            if (Raycast(rayBotTowardsDirection, new Vector3(Mathf.Sign(currentDir.x), 0, 0), 0.5f))
            {
                GroundCheckActions();
            }
            else if (Raycast(rayTopTowardsDirection, new Vector3(Mathf.Sign(currentDir.x), 0, 0), 0.5f))
            {
                GroundCheckActions();
            }

            if (!Raycast(rayRightDown, Vector3.down, 1.0f))
            {
                GroundCheckActions();
            }

            else if (!Raycast(rayLeftDown, Vector3.down, 1.0f))
            {
                GroundCheckActions();
            }
        }
    }

    void UpdateIfStillStopped()
    {
        if (stoppedTimer >= TIME_SPENT_STILL_BEFORE_TURNING)
        {
            stoppedTimer = 0.0f;
            stopped = false;
        }
    }

    void GroundCheckActions()
    {
        if (!stoppedBefore)
        {
            Debug.Log("Enemy stops");
            stoppedBefore = true;
            stopped = true;
        }

        if (!stopped)
        {
            if (!playerVisible)
            {
                FlipAround();
                stoppedBefore = false;
            }
        }
    }

    bool Raycast(Vector3 pos, Vector3 direction, float length) //terrain
    {
        //user layer 8 on terraincollision
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
        if (visionResetTimer >= VISION_RESET_TIME)
        {
            visionResetTimer = 0.0f;
            visionAllowedToReset = true;
        }
    }
}