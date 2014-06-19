using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour
{
    public SpiderAIState State { get; private set; }
    public float regularSpeed;
    public float attackSpeed;

    private GameObject player;
    private Vector2 targetPosition;
    private Rigidbody2D spiderRigidbody;
    private CircleCollider2D webCollider;
    private float spiderWebRadius;
    private SpiderAnimation animation;

    private bool getTarget = true;

    private void Start()
    {
        State = SpiderAIState.Moving;
        player = GameObject.Find("Player");
        spiderRigidbody = GetComponent<Rigidbody2D>();
        webCollider = transform.parent.GetComponent<CircleCollider2D>();
        spiderWebRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        animation = GetComponentInChildren<SpiderAnimation>();
        NewRandomTarget();
        RotateTowardsTarget();
    }

    private void Update()
    {
        switch (State)
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
        State = spiderAIState;
        animation.SwitchAnimationTo(spiderAIState);

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
        Invoke("StartMoving", Random.Range(4.0F, 8.0F));
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
        float step = regularSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
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
        float step = attackSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
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

        float angleDegrees = angleRadians * (180 / Mathf.PI) + 90.0F;

        transform.rotation = Quaternion.identity;
        transform.Rotate(0.0F, 0.0F, angleDegrees);
    }
}
