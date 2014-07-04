using UnityEngine;
using System.Collections;

public class HangingSpider : MonoBehaviour
{
    public Vector3 endPosition;
    public float regularSpeed;
    public float attackSpeed;
    public HangingSpiderState State { get; private set; }

    private LineRenderer web;
    private Vector3 startPosition;

    private void Start()
    {
        State = HangingSpiderState.Idle;
        web = GetComponent<LineRenderer>();
        web.SetPosition(0, startPosition);
        web.SetPosition(1, startPosition);

        startPosition = transform.position;
    }

    private void Update()
    {
        switch (State)
        {
            case HangingSpiderState.Idle:
                UpdateIdle();
                break;

            case HangingSpiderState.Attacking:
                UpdateAttacking();
                break;

            case HangingSpiderState.Retreating:
                UpdateRetreating();
                break;
        }

        DrawWeb();
    }

    public void SwitchTo(HangingSpiderState state)
    {
        if (State != state)
            State = state;

        switch (state)
        {
            case HangingSpiderState.Idle:
                break;

            case HangingSpiderState.Attacking:
                break;

            case HangingSpiderState.Retreating:
                break;
        }
    }

    private void DrawWeb()
    {
        web.SetPosition(1, new Vector3(0, Mathf.Abs((startPosition.y - transform.position.y)), 0));
    }

    #region Idle

    private void UpdateIdle()
    {
    }

    #endregion Idle

    #region Attacking

    private void UpdateAttacking()
    {
        if (transform.position != endPosition)
        {
            float step = attackSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        }
        else
        {
            Invoke("ChangeToRetreating", 2.0F);
        }
    }

    private void ChangeToRetreating()
    {
        SwitchTo(HangingSpiderState.Retreating);
    }

    #endregion Attacking

    #region Retreating

    private void UpdateRetreating()
    {
        if (transform.position != startPosition)
        {
            float step = regularSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
        }
        else
        {
            SwitchTo(HangingSpiderState.Idle);
        }
    }

    #endregion Retreating
}
