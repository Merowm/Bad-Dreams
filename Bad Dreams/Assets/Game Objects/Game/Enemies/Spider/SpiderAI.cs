using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour
{
    public float regularSpeed;
    public float attackSpeed;

    private SpiderAIState state;
    private GameObject player;
    private Vector2 targetPosition;
    private Rigidbody2D spiderRigidbody;
    private CircleCollider2D webCollider;
    private float spiderWebRadius;

    private bool getTarget = true;

    private void Start()
    {
        state = SpiderAIState.Moving;
        player = GameObject.Find("Player");
        spiderRigidbody = GetComponent<Rigidbody2D>();
        webCollider = transform.parent.GetComponent<CircleCollider2D>();
        spiderWebRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        NewRandomTarget();
        RotateTowardsTarget();
    }

    private void Update()
    {
        switch (state)
        {
            case SpiderAIState.Idle:
                UpdateIdle();
                break;

            case SpiderAIState.Moving:
                UpdateMoving();
                break;

            case SpiderAIState.Attacking:
                UpdateAttacking();
                break;
        }
    }

    public void SwitchTo(SpiderAIState spiderAIState)
    {
        state = spiderAIState;

        switch (spiderAIState)
        {
            case SpiderAIState.Idle:
                OnSwitchToIdle();
                break;

            case SpiderAIState.Moving:
                OnSwitchToMoving();
                break;

            case SpiderAIState.Attacking:
                OnSwitchToAttacking();
                break;
        }
    }

    #region Idle

    private void OnSwitchToIdle()
    {
        Invoke("StartMoving", Random.Range(1.0F, 3.0F));
    }

    private void UpdateIdle()
    {
    }

    private void StartMoving()
    {
        SwitchTo(SpiderAIState.Moving);
    }

    #endregion Idle

    #region Moving

    private void OnSwitchToMoving()
    {
        NewRandomTarget();
        RotateTowardsTarget();
    }

    private void UpdateMoving()
    {
        MovingUpdatePosition();

        if (getTarget && IsNearTarget)
        {
            NewRandomTarget();
            RotateTowardsTarget();
            getTarget = false;
        }

        if (IsNearTarget)
        {
            getTarget = true;
            SwitchTo(SpiderAIState.Idle);
        }
    }

    private void MovingUpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * regularSpeed);
    }

    private float isNearThreshold = 0.2F;

    private bool IsNearTarget
    {
        get
        {
            return Vector3.Distance(transform.position, targetPosition) <= isNearThreshold;
        }
    }

    #endregion Moving

    #region Attacking

    private void OnSwitchToAttacking()
    {
        
    }

    private void UpdateAttacking()
    {
        AttackingUpdatePosition();
        AttackPlayer();
    }

    private void AttackingUpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * attackSpeed);
    }

    private void AttackPlayer()
    {
        targetPosition = player.transform.position;
        RotateTowardsTarget();
    }

    #endregion Attacking

    private void NewRandomTarget()
    {
        targetPosition = transform.parent.position + new Vector3(
                Random.Range(-spiderWebRadius, spiderWebRadius),
                Random.Range(-spiderWebRadius, spiderWebRadius),
                0.0F);
    }

    private void RotateTowardsTarget()
    {
        float angleRadians = Mathf.Atan2(
            (targetPosition.y - transform.position.y),
            (targetPosition.x - transform.position.x));

        float angleDegrees = angleRadians * (180 / Mathf.PI);

        transform.rotation = Quaternion.identity;
        transform.Rotate(0.0F, 0.0F, angleDegrees);
    }
}
