using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationController
{
    //지금은 플레이어가 바로 달리고 있다
    private static readonly int isRun = Animator.StringToHash("Run");

    private static readonly int isOnJump = Animator.StringToHash("OnJump");
    private static readonly int isJump = Animator.StringToHash("Jump");

    private static readonly int isHit = Animator.StringToHash("Hit");
    private static readonly int isSlow = Animator.StringToHash("Slow");

    private static readonly int isDead = Animator.StringToHash("OnDead");

    //전투 기믹 추가용
    private static readonly int isOnAttack = Animator.StringToHash("OnAttack");
    private static readonly int isAttack = Animator.StringToHash("Attack");

    protected override void Awake()
    {
        base.Awake();
    }

    public void Jump()
    {
        animator.SetTrigger(isOnJump);
        animator.SetBool(isJump, true);
    }

    public void JumpEnd()
    {
         animator.SetBool(isJump, false);
    }

    public bool GetJump()
    {
        return animator.GetBool(isJump);
    }

    public void Hit()
    {
        animator.SetBool(isHit, true);
    }

    public void HitEnd()
    {
        animator.SetBool(isHit, false);
    }

    public void Slow()
    {
        float slowtime = Time.time;
        animator.SetFloat(isSlow, slowtime);
    }

    public void Run()
    {
        animator.SetBool(isRun, true);
    }

    public void RunEnd()
    {
        animator.SetBool(isRun, false);
    }

    public void OnDead()
    {
        animator.SetTrigger(isDead);
    }
}
