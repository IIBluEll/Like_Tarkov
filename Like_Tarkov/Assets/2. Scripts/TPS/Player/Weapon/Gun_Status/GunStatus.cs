using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStatus : MonoBehaviour
{
    protected BulletCasing_Pool bulletCasingPool;
    protected Bullet_MemoryPool bulletPool;
    protected AudioSource gunAudioSource;
    protected Impact_MemoryPool impactMemoryPool;
    
    [SerializeField] protected PlayerAnimationController playerAnim;

    [Header("  ## AUDIO CLIPS ##")] 
    [SerializeField] protected AudioClip audioClip_FIRE;
    [SerializeField] protected AudioClip audioClip_Reload;
    
    [Space(10f), Header(" ## FIRE EFFECT ##  "),SerializeField]
    protected GameObject muzzleFlashEffect;

    [Space(10f), Header(" ## BULLET SPAWN POINT ##"), SerializeField]
    protected Transform bulletSpawnPoint;
    
    [Space(10f), Header(" ## CASING SPAWN POINT ##"), SerializeField] 
    protected Transform casingSpawnPoint;

    [Space(10f), Header(" ## WEAPON SETTING ##"), SerializeField]
    protected TSP_WeaponSetting weaponSetting;
    
    protected bool isReload = false;      // 재장전중인지?
    protected float lastAttackTime = 0;   // 마지막 발사 시간
    
    [SerializeField] protected Camera Camera;
    [SerializeField] protected GameObject bulletPrefeb;

    protected virtual void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();
        bulletCasingPool = GetComponent<BulletCasing_Pool>();
        bulletPool = GetComponent<Bullet_MemoryPool>();
        impactMemoryPool = GetComponent<Impact_MemoryPool>();
        
        // 처음 탄 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    
    protected virtual void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
    }
    
    public virtual void StartWeaponAction(int type = 0)
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
    
    public virtual void StartReload()
    {
        // 현재 재장전 중이면 재장전 불가능
        if (isReload == true) return;
        
        // 무기 액션 도중에 'R' 키를 눌러 재장전을 시도하면 무기 액션 종료 후 재장전
        StopWeaponAction();
        StartCoroutine("OnReload");
    }
    
    public virtual void StopWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    
    protected virtual IEnumerator OnReload()
    {
        isReload = true;
        
        // 재장전 애니메이션, 사운드 재생
        playerAnim.OnReload();
        PlaySound(audioClip_Reload);

        while (true)
        {
            // 사운드가 재생중이 아니고, 현재 애니메이션이 " Idle " 이면
            // 재장전 애니메이션, 사운드 재생이 종료되었다는 것
            if (gunAudioSource.isPlaying == false && playerAnim.CurrentAnimationIs("Idle",1))
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
    
    protected virtual IEnumerator OnAttackLoop()
       {
          while (true)
          {
             OnAttack();
    
             yield return null;
          }
       }
    
    protected virtual IEnumerator OnMuzzleFlashEffect()
       {
          muzzleFlashEffect.SetActive(true);
    
          yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
          
          muzzleFlashEffect.SetActive(false);
       }
    
       public virtual void OnAttack()
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
             
             // 레이를 발사해 원하는 위치 공격
             TwoStepRayCast();
          }
       }

       protected virtual void TwoStepRayCast()
       {
           Ray ray;
           RaycastHit hit;
           var targetPoint = Vector3.zero;
           
           // 화면의 중앙 좌표
           ray = Camera.ViewportPointToRay(Vector2.one * .5f);
           
           // 공격 사거리 안에 부딪히는 오브젝트가 있으면 targetPoint는 부딪힌 위치
           if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
           {
               targetPoint = hit.point;
           }
           // 공격 사거리안에 부딪히는 오브젝트가 없다면 targetPoint는 최대 사거리 위치
           else
           {
               targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
           }
           
           // Debug
           Debug.DrawRay(ray.origin,ray.direction * weaponSetting.attackDistance, Color.red);
           
           // 첫번째 Ray로 얻어진 targetPoint를 목표지점으로 설정하고,
           // 총구를 시작지점으로 하여 재연산

           var attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;

           bulletPool.SpawnBullet(bulletSpawnPoint.position, 
                            Quaternion.LookRotation(attackDirection) * Quaternion.Euler(90, 0, 0), 
                                    attackDirection);
           
           // if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
           // {
           //     impactMemoryPool.SpawnImpact(hit);
           // }
            // Debug         
           Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
       }
       
       protected virtual void PlaySound(AudioClip clip)
       {
          gunAudioSource.Stop();
          gunAudioSource.clip = clip;
          gunAudioSource.Play();
       }
}
