using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_AussultRifle : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController playerAnim;

    [Header("  ## AUDIO CLIPS ##")] 
    [SerializeField] private AudioClip audioClip_FIRE;
    [SerializeField] private AudioClip audioClip_Reload;

    [Space(10f), Header(" ## FIRE EFFECT ##  "),SerializeField]
    private GameObject muzzleFlashEffect;

    [Space(10f), Header(" ## BULLET CASING SPAWN POINT ##"), SerializeField] 
    private Transform casingSpawnPoint;

    [Space(10f), Header(" ## WEAPON SETTING ##"), SerializeField]
    private TSP_WeaponSetting weaponSetting;

    private bool isReload = false;      // 재장전중인지?
    private float lastAttackTime = 0;   // 마지막 발사 시간

    private BulletCasing_Pool bulletCasingPool;
    private AudioSource gunAudioSource;


    private void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();
        bulletCasingPool = GetComponent<BulletCasing_Pool>();
        
        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
    }

    public void StartWeaponAction(int type = 0)
    {
        if (isReload == true) return;
        
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

    public void StartReload()
    {
        // 현재 재장전 중이면 재장전 불가능
        if (isReload == true) return;
        
        // 무기 액션 도중에 'R' 키를 눌러 재장전을 시도하면 무기 액션 종료 후 재장전
        StopWeaponAction();
        StartCoroutine("OnReload");
    }
    
    public void StopWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }

    private IEnumerator OnReload()
    {
        isReload = true;
        
        // 재장전 애니메이션, 사운드 재생
        playerAnim.OnReload();
        PlaySound(audioClip_Reload);

        while (true)
        {
            // 사운드가 재생중이 아니고, 현재 애니메이션이 " Idle " 이면
            // 재장전 애니메이션, 사운드 재생이 종료되었다는 것
            if (gunAudioSource.isPlaying == false && playerAnim.CurrentAnimationIs("Idle"))
            {
                Debug.Log("Reload 완료");
                
                isReload = false;
                
                // 현재 탄 수를 최대로 설정
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                
                yield break;
            }

            yield return null;
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
              lastAttackTime = Time.time;
         
              // 탄이 없으면 공격 불가능
              if (weaponSetting.currentAmmo <= 0)
              {
                  return;
              }
              
              weaponSetting.currentAmmo--;
             
             playerAnim.Play("Fire", 1, 0);
    
             StartCoroutine("OnMuzzleFlashEffect");
             
             PlaySound(audioClip_FIRE);
             
             //탄피 생성
             bulletCasingPool.Spwancasing(casingSpawnPoint.position, transform.right);
          }
       }
    
       private void PlaySound(AudioClip clip)
       {
          gunAudioSource.Stop();
          gunAudioSource.clip = clip;
          gunAudioSource.Play();
       }
}
