using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AssultRifle : MonoBehaviour
{
   [SerializeField] 
   private PlayerAnimationController playerAnim;

   [Header(" ## AUDIO CLIPS ## ")] 
   [SerializeField] private AudioClip audioClipFire;
   
   [Space(10f), Header(" ## FIRE EFFECT ##")] 
   [SerializeField]
   private GameObject muzzleFlashEffect;

   [Space(10f), Header(" ## BULLET CASING SPWAN POINT ##")] 
   [SerializeField]
   private Transform casingSpwanPoint;
   
   [Space(10f), Header(" ## WEAPON SETTING ##")]
   [SerializeField] 
   private WeaponSetting weaponSetting;   // 무기 설정

   private float lastAttackTime = 0;      // 마지막 발사 시간

   private BulletCasing_Pool bulletCasingPool;
   private AudioSource gunAudioSource;
   
   private void Awake()
   {
      gunAudioSource = GetComponent<AudioSource>();
      bulletCasingPool = GetComponent<BulletCasing_Pool>();
   }

   private void OnEnable()
   {
      muzzleFlashEffect.SetActive(false);
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

   private IEnumerator OnMuzzleFlashEffect()
   {
      muzzleFlashEffect.SetActive(true);

      yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
      
      muzzleFlashEffect.SetActive(false);
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

         StartCoroutine("OnMuzzleFlashEffect");
         
         PlaySound(audioClipFire);
         
         //탄피 생성
         bulletCasingPool.Spwancasing(casingSpwanPoint.position, transform.right);
      }
   }

   private void PlaySound(AudioClip clip)
   {
      gunAudioSource.Stop();
      gunAudioSource.clip = clip;
      gunAudioSource.Play();
   }
}
