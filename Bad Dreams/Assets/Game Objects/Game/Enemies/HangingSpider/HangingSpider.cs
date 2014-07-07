using UnityEngine;
using System.Collections;

public class HangingSpider : MonoBehaviour
{
    public Vector3 endPosition;
    public float ascendSpeed;
    public float descendSpeed;
    public HangingSpiderState State { get; private set; }

    private LineRenderer web;
    private Vector3 startPosition;
    private Animator animator;

    private void Start()
    {
        web = GetComponent<LineRenderer>();
        web.SetPosition(0, startPosition);
        web.SetPosition(1, startPosition);
        startPosition = transform.position;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("ascending", false);
        animator.SetBool("descending", false);
        animator.SetBool("attacking", false);
        animator.SetBool("idle", true);

        SwitchTo(HangingSpiderState.Idle);
    }

    private void Update()
    {
        switch (State)
        {
            case HangingSpiderState.Idle:
                UpdateIdle();
                break;

            case HangingSpiderState.Descending:
                UpdateDescending();
                break;

            case HangingSpiderState.Attacking:
                UpdateAttacking();
                break;

            case HangingSpiderState.Ascending:
                UpdateAscending();
                break;
        }

        DrawWeb();
    }

    public void SwitchTo(HangingSpiderState state)
    {
        if (State != state)
        {
            State = state;
            UpdateAnimation();
        }
    }

    private void DrawWeb()
    {
        web.SetPosition(1, new Vector3(0, Mathf.Abs((startPosition.y - transform.position.y)), 0));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<HitAnimation>().ActivateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        switch (State)
        {
            case HangingSpiderState.Idle:
                animator.speed = 1.0F;
                animator.SetBool("descending", false);
                animator.SetBool("idle", true);
                break;

            case HangingSpiderState.Descending:
                animator.speed = 0.0F;
                animator.SetBool("idle", false);
                animator.SetBool("descending", true);
                break;

            case HangingSpiderState.Ascending:
                animator.speed = 0.0F;
                animator.SetBool("attacking", false);
                animator.SetBool("ascending", true);
                break;

            case HangingSpiderState.Attacking:
                animator.speed = 1.0F;
                animator.SetBool("descending", false);
                animator.SetBool("attacking", true);
                break;
        }
    }

    public void Reset()
    {
        SwitchTo(HangingSpiderState.Idle);
        transform.position = startPosition;
    }

    #region Idle

    private void UpdateIdle()
    {
    }

    #endregion Idle

    #region Descending

    private void UpdateDescending()
    {
        if (transform.position != endPosition)
        {
            float step = descendSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        }
        else
        {
            SwitchTo(HangingSpiderState.Attacking);
        }
    }

    #endregion Descending

    #region Attacking

    private bool invoked = false;

    private void UpdateAttacking()
    {
        if (!invoked)
        {
            Invoke("ChangeToAscending", 4.0F);
            invoked = true;
        }
    }

    private void ChangeToAscending()
    {
        invoked = false;
        SwitchTo(HangingSpiderState.Ascending);
    }

    #endregion Attacking

    #region Ascending

    private void UpdateAscending()
    {
        if (transform.position != startPosition)
        {
            float step = ascendSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
        }
        else
        {
            SwitchTo(HangingSpiderState.Idle);
        }
    }

    #endregion Ascending
}
