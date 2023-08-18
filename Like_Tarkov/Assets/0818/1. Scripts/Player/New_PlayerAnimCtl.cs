using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerAnimCtl : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void AnimatorValue(float horizontalMove, float verticalMove)
    {
        
    }
}
