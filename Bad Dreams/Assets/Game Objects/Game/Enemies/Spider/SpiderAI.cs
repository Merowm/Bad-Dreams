using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour
{
    public SpiderAIState State { get; private set; }
    public SpiderAIState PreviousState { get; private set; }
    public float regularSpeed;
    public float chasingSpeed;
    public float minimumIdleLength;
    public float maximumIdleLength;

    private float attackSpeed;
    private GameObject player;
    private Vector2 targetPosition;
    private Rigidbody2D spiderRigidbody;
    private CircleCollider2D webCollider;
    private float spiderWebRadius;
    private SpiderAnimation animation;
    private Vector3 startPosition;

    private bool getTarget = true;

    private void Start()
    {
        State = SpiderAIState.Moving;
        PreviousState = State;
        attackSpeed = 0.8F;
        player = GameObject.Find("Player");
        spiderRigidbody = GetComponent<Rigidbody2D>();
        webCollider = transform.parent.GetComponent<CircleCollider2D>();
        spiderWebRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        animation = GetComponentInChildren<SpiderAnimation>();
        startPosition = transform.position;
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

            case SpiderAIState.Chasing:
                UpdateChasing();
                break;

            case SpiderAIState.Attacking:
                UpdateAttacking();
                break;
        }
    }

    public void SwitchTo(SpiderAIState spiderAIState)
    {
        if (State == spiderAIState)
            return;

        PreviousState = State;
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

            case SpiderAIState.Chasing:
                OnSwitchToChasing();
                break;

            case SpiderAIState.Attacking:
                OnSwitchToAttacking();
                break;
        }
    }

    #region Idle

    private float idleTimer = 0.0F;
    private float idleDuration = 0.0F;

    private void OnSwitchToIdle()
    {
        idleTimer = 0.0F;
        idleDuration = Random.RandomRange(minimumIdleLength, maximumIdleLength);
    }

    private void UpdateIdle()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
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

        if (IsNearTarget)
        {
            getTarget = true;
            SwitchTo(SpiderAIState.Idle);
            return;
        }

        if (getTarget && IsNearTarget)
        {
            NewRandomTarget();
            RotateTowardsTarget();
            getTarget = false;
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

    #region Chasing

    private void OnSwitchToChasing()
    {
    }

    private void UpdateChasing()
    {
        ChasePlayer();
        ChasingUpdatePosition();
        AttackPlayerIfNear();
    }

    private void ChasingUpdatePosition()
    {
        float step = chasingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    private void ChasePlayer()
    {
        targetPosition = player.transform.position;
        RotateTowardsTarget();
    }

    private float attackDistance = 0.6F;

    private void AttackPlayerIfNear()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            SwitchTo(SpiderAIState.Attacking);
        }
    }

    private float playerOutsideOffset = 0.5F;

    private void StopIfPlayerOutsideWeb()
    {
        if (Vector3.Distance(player.transform.position, transform.parent.transform.position) > spiderWebRadius + playerOutsideOffset )
        {
            SwitchTo(SpiderAIState.Idle);
        }
    }

    #endregion Chasing

    #region Attacking

    private float attackTimer = 0.0F;
    private float attackDuration = 2.0F;

    private void OnSwitchToAttacking()
    {
        attackTimer = 0.0F;
    }

    private void UpdateAttacking()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackDuration)
        {
            SwitchTo(SpiderAIState.Chasing);
        }

        AttackPlayer();
        //AttackingUpdatePosition();
    }

    private void AttackPlayer()
    {
        targetPosition = player.transform.position;
        RotateTowardsTarget();
    }

    private void AttackingUpdatePosition()
    {
        float step = attackSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
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

    public void Reset()
    {
        transform.position = startPosition;
        SwitchTo(SpiderAIState.Idle);
    }
}
