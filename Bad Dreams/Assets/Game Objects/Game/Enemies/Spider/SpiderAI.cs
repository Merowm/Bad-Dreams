using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour
{
    public SpiderAIState State { get; set; }

    private Vector2 targetPosition;
    private Rigidbody2D spiderRigidbody;
    private CircleCollider2D webCollider;
    private int spiderWebRadius;

    private void Start()
    {
        State = SpiderAIState.Idle;
        spiderRigidbody = GetComponent<Rigidbody2D>();
        webCollider = transform.parent.GetComponent<CircleCollider2D>();
        spiderWebRadius = (int)transform.parent.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
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

    #region Idle

    private bool getTarget = true;

    private void UpdateIdle()
    {
        if (getTarget)
        {
            Invoke("NewTarget", 3.0F);// NewTarget();
            getTarget = false;
        }


        Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

        //State = SpiderAIState.Moving;
    }

    #endregion Idle

    #region Moving

    private void UpdateMoving()
    {
        MovingUpdatePosition();
    }

    private void MovingUpdatePosition()
    {

    }

    #endregion Moving

    #region Attacking

    private void UpdateAttacking()
    {
        AttackingUpdatePosition();
    }

    private void AttackingUpdatePosition()
    {
    }

    #endregion Attacking

    private void NewTarget()
    {
        int count = 0;

        do
        {
            if (count > 50)
                break;

            count++;
            targetPosition = new Vector2(
                transform.position.x + Random.Range(-spiderWebRadius, spiderWebRadius),
                transform.position.y + Random.Range(-spiderWebRadius, spiderWebRadius));
        }
        while (!webCollider.OverlapPoint(targetPosition));

        float angleRadians = Mathf.Atan2(
            (targetPosition.y - transform.position.y),
            (targetPosition.x - transform.position.x));

        float angleDegrees = angleRadians * (180 / Mathf.PI);

        transform.Rotate(0.0F, 0.0F, angleDegrees);
        getTarget = true;
    }
}
