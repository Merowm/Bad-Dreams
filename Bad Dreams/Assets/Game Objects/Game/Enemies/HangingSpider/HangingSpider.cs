using UnityEngine;
using System.Collections;

public class HangingSpider : MonoBehaviour
{
    public Vector3 endPosition;
    public float ascendSpeed;
    public float descendSpeed;
    public HangingSpiderState State { get; private set; }

    private LineRenderer web;
    private float webOffset = 0.68F;
    private Vector3 startPosition;
    private Animator animator;

    private HidingSkill player;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        web = GetComponent<LineRenderer>();
        web.SetPosition(0, new Vector3(startPosition.x, startPosition.y + webOffset));
        web.SetPosition(1, new Vector3(startPosition.x, startPosition.y + webOffset));
        web.sortingLayerName = "Player Foreground";
        web.sortingOrder = -1;
        startPosition = transform.position;

        animator = GetComponentInChildren<Animator>();
        animator.SetBool("ascending", false);
        animator.SetBool("descending", false);
        animator.SetBool("attacking", false);
        animator.SetBool("idle", true);

        player = GameObject.Find("Player").GetComponent<HidingSkill>();

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
        PlayerCollision();
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
        if ((startPosition.y - transform.position.y) > webOffset)
            web.SetPosition(1, new Vector3(0, Mathf.Abs((startPosition.y - transform.position.y)), 0));
    }

    private void PlayerCollision()
    {
        if (player.IsHiding)
            Physics2D.IgnoreLayerCollision(9, 10, true);
        else
            Physics2D.IgnoreLayerCollision(9, 10, false);
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
                animator.speed = 0.0F;
                animator.SetBool("attacking", false);
                animator.SetBool("ascending", false);
                animator.SetBool("descending", false);
                animator.SetBool("idle", true);
                break;

            case HangingSpiderState.Descending:
                animator.speed = 0.0F;
                animator.SetBool("attacking", false);
                animator.SetBool("ascending", false);
                animator.SetBool("descending", true);
                animator.SetBool("idle", false);
                break;

            case HangingSpiderState.Ascending:
                animator.speed = 0.0F;
                animator.SetBool("attacking", false);
                animator.SetBool("ascending", true);
                animator.SetBool("descending", false);
                animator.SetBool("idle", false);
                break;

            case HangingSpiderState.Attacking:
                animator.speed = 1.0F;
                animator.SetBool("attacking", true);
                animator.SetBool("ascending", false);
                animator.SetBool("descending", false);
                animator.SetBool("idle", false);
                break;
        }
    }

    public void Reset()
    {
        SwitchTo(HangingSpiderState.Idle);
        transform.position = startPosition;
        web.SetPosition(0, new Vector3(startPosition.x, startPosition.y + webOffset));
        web.SetPosition(1, new Vector3(startPosition.x, startPosition.y + webOffset));
    }

    #region Idle

    private bool idleAnimationSet = false;
    private float idleAnimationTimer = 0.0F;
    private float idleAnimationInterval = 0.0F;

    private void UpdateIdle()
    {
        if (!idleAnimationSet)
        {
            if (animator.speed == 1.0F)
                idleAnimationInterval = 1.0F;
            else
                idleAnimationInterval = Random.Range(7.5F, 15.0F);

            idleAnimationSet = true;
        }

        if (idleAnimationSet)
        {
            idleAnimationTimer += Time.deltaTime;
            if (idleAnimationTimer > idleAnimationInterval)
            {
                ToggleIdleAnimation();
                idleAnimationSet = false;
                idleAnimationTimer = 0.0F;
            }
        }
    }

    private void ToggleIdleAnimation()
    {
        animator.speed = (animator.speed == 1.0F) ? 0.0F : 1.0F;
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
            Invoke("ChangeToAscending", 0.5F);
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
