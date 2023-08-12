using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
   private Animator playerAnimator;
   
   private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
   private static readonly int XDir = Animator.StringToHash("xDir");
   private static readonly int YDir = Animator.StringToHash("yDir");
   private static readonly int Reload = Animator.StringToHash("OnReload");
   private static readonly int Avoid = Animator.StringToHash("OnAvoid");

   private void Awake()
   {
      playerAnimator = GetComponent<Animator>();
   }

   public float AnimMoveSpeed
   {
      get => playerAnimator.GetFloat(MoveSpeed);
      set => playerAnimator.SetFloat(MoveSpeed, value);
   }

   public float DirX
   {
      get => playerAnimator.GetFloat(XDir);
      set => playerAnimator.SetFloat(XDir,value);
   }
   
   public float DirY
   {
      get => playerAnimator.GetFloat(YDir);
      set => playerAnimator.SetFloat(YDir,value);
   }

   public void OnReload()
   {
      playerAnimator.SetTrigger(Reload);
   }

   public void Play(string stateName, int layer, float normalizedTime)
   {
      playerAnimator.Play(stateName, layer, normalizedTime);
   }

   public bool CurrentAnimationIs(string name, int layernum)
   {
      return playerAnimator.GetCurrentAnimatorStateInfo(layernum).IsName(name);
   }
}
