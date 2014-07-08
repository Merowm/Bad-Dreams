using UnityEngine;
using System.Collections;

public class SpiderAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("moving", true);
        animator.SetBool("attacking", false);
        animator.SetBool("idle", false);
        animator.SetBool("chasing", false);
    }

    public void SwitchAnimationTo(SpiderAIState state)
    {
        switch (state)
        {
            case SpiderAIState.Idle:
                animator.SetBool("moving", false);
                animator.SetBool("attacking", false);
                animator.SetBool("chasing", false);
                animator.SetBool("idle", true);
                break;

            case SpiderAIState.Moving:
                animator.SetBool("moving", true);
                animator.SetBool("attacking", false);
                animator.SetBool("idle", false);
                animator.SetBool("chasing", false);
                break;

            case SpiderAIState.Chasing:
                animator.SetBool("chasing", true);
                animator.SetBool("idle", false);
                animator.SetBool("attacking", false);
                animator.SetBool("moving", false);
                break;

            case SpiderAIState.Attacking:
                animator.SetBool("moving", false);
                animator.SetBool("attacking", true);
                animator.SetBool("idle", false);
                animator.SetBool("chasing", false);
                break;
        }
    }
}
