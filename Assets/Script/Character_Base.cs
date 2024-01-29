using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    Animator animator;
    Transform transform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }
    public void PlayWalkingAnimation(Vector3 animationDir)
    {
        animator.SetBool("IsWalking", true);
    }

    public void PlayIdleAnimation(Vector3 animationDir)
    {
        animator.SetBool("IsWalking", false);

    }

    public void PlayAttackAnimation(Vector3 animationDir)
    {
        animator.SetTrigger("SwordAttack");
        
    }

    public void PlayDeadAnimation(Vector3 animationDir)
    {
        animator.SetTrigger("IsDead");
    }

    public void SetTransform(bool flipTransform)
    {
        if (flipTransform)
        {
            transform.transform.localScale = new Vector2(-1, 1);
        } else
        {
            transform.transform.localScale = new Vector2(1, 1);

        }

    }
}
