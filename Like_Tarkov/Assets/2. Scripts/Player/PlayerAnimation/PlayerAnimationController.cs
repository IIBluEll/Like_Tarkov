using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
   private Animator playerAnimator;
   private static readonly int CheckMovement = Animator.StringToHash("CheckMovement");

   private void Awake()
   {
      playerAnimator = GetComponent<Animator>();
   }

   public float AnimMoveSpeed
   {
      get => playerAnimator.GetFloat(CheckMovement);
      set => playerAnimator.SetFloat(CheckMovement, value);
   }

   public void Play(string stateName, int layer, float normalizedTime)
   {
      playerAnimator.Play(stateName, layer, normalizedTime);
   }
}
