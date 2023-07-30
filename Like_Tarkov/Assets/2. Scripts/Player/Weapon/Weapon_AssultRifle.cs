using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssultRifle : MonoBehaviour
{
   [SerializeField] private PlayerAnimationController playerAnim;
   
   //[Header(" ## AUDIO CLIPS ## ")]
   //TODO 차후 총기 사운드 추가

   [Space(10f), Header(" ## WEAPON SETTING ##")]
   [SerializeField] 
   private WeaponSetting weaponSetting;   // 무기 설정

   private float lastAttackTime = 0;      // 마지막 발사 시간
   
   private void Awake()
   {
      
   }

   public void StartWeaponAction(int type = 0)
   {
      // 마우스 왼쪽 클릭
      if (type == 0)
      {
         // 연속 발사
         if (weaponSetting.isAutomaticAttack == true)
         {
            StartCoroutine("OnAttackLoop");
         }
         // 단발
         else
         {
            OnAttack();
         }
      }
   }

   public void StopWeaponAction(int type = 0)
   {
      // 마우스 왼쪽 클릭
      if (type == 0)
      {
         StopCoroutine("OnAttackLoop");
      }
   }

   private IEnumerator OnAttackLoop()
   {
      while (true)
      {
         OnAttack();

         yield return null;
      }
   }

   public void OnAttack()
   {
      if (Time.time - lastAttackTime > weaponSetting.attackRate)
      {
         // 뛰고 있을 때는 공격 불가능
         if (playerAnim.AnimMoveSpeed > 5f)
         {
            return;
         }

         lastAttackTime = Time.time;
         playerAnim.Play("Fire", -1, 0);
      }
   }
}
