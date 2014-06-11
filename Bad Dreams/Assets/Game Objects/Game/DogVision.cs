using UnityEngine;
using System.Collections;

public class DogVision : MonoBehaviour
{
    const float VISION_UPDATE_INTERVAL = 0.0f;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT = 2;
    const int VISION_ALERTNESS_ADDITION_ON_SIGHT_WHILE_ALERTED = 4;
    const int VISION_ALERTNESS_DECREASE_ON_LOSE_SIGHT = 1;
    const int VISION_BECOME_ALERTED_LIMIT = 200;
    const int ENEMY_LAYER = 9;
    const int HIDDEN_LAYER = 10;

    GameObject player;
    SpriteRenderer spriteRend;
    Vector3 currentDir;
    float playerDistance, viewLength, viewAngle, alertTimer, lastVisionCheckTime;
    int alertness;
    bool playerVisible, alerted;

    void Start()
    {
        viewLength = 3.0f;
        viewAngle = 55.0f;
        player = GameObject.Find("Player");
        spriteRend = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        currentDir = new Vector3(spriteRend.transform.localScale.x, 0, 0);
        alertness = 0;
        alertTimer = 0.0f;
        playerVisible = false;
        alerted = false;
        lastVisionCheckTime = 0.0f;
    }

    void Update()
    {
        //dogmovement

        if (alerted)
        {
            if (playerVisible)
            {
            }
            else
            {
                transform.position += new Vector3(currentDir.x * 2 * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if (playerVisible)
            {
            }
            else
            {
                transform.position += new Vector3(currentDir.x * Time.deltaTime, 0, 0);
            }
        }

        GroundCheck();




        lastVisionCheckTime += Time.deltaTime;

        if (lastVisionCheckTime >= VISION_UPDATE_INTERVAL)
        {
            ResetVisionTime();
            PlayerVisibilityCheck();
            UpdateAlertness();
            DEBUG_UPDATECOLOR();
        }

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
    }

    void FlipAround()
    {
        spriteRend.transform.localScale = new Vector3(-spriteRend.transform.localScale.x, 1, 1);

        currentDir = new Vector3(spriteRend.transform.localScale.x, 0, 0);
    }

    void ChangePlayerLayer()
    {
        if (player.gameObject.layer == HIDDEN_LAYER)
        {
            Debug.Log("Player comes out from hiding");
            player.gameObject.layer = 0;
        }

        else
        {
            Debug.Log("Player hides");
            player.gameObject.layer = HIDDEN_LAYER;
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
                    if (player.gameObject.layer == 9) //if player is in the hiding layer
                    {
                        if (playerVisible)
                        {
                            if (alerted)
                            {
                                Debug.Log("Hidden in plain sight");
                            }
                            else
                            {
                                playerVisible = false;
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
                    }
                }
                else playerVisible = false;
            }
            else playerVisible = false;
        }
        else playerVisible = false;
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
        lastVisionCheckTime = 0.0f;
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

        if (Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position += new Vector3(0, Time.deltaTime * 2.0f, 0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            player.transform.position += new Vector3(0, -Time.deltaTime * 2.0f, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.position += new Vector3(-Time.deltaTime * 2.0f, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.position += new Vector3(Time.deltaTime * 2.0f, 0, 0);
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

    //requires player to have a RigidBody2D
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " hits the enemy collider");
        if (col.gameObject.name == "Player")
        {
            if (alertness < 100)
            {
                alertness = 100;
            }
            FacePlayer();
        }
    }

    void FacePlayer()
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

    //movement methods

    void GroundCheck()
    {
        
        Vector3 rayPosRight = transform.position + new Vector3(spriteRend.bounds.max.x / 100, 0, 0);
        Debug.DrawRay(rayPosRight, Vector3.down);
        if (!Raycast(rayPosRight, Vector3.down, 1.0f))
        {
            FlipAround();
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
}