using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType {Normal = 0, Obstacle, }

public class Impact_MemoryPool : MonoBehaviour
{
   [SerializeField] private GameObject[] impactPrefab;   // 피탄 이펙트

   private MemoryPool[] memoryPool;                      // 피탄 이펙트 메모리풀

   private void Awake()
   {
      // 피격 이벤트가 여러 종류이면 종류별로 memoryPool 생성 
      memoryPool = new MemoryPool[impactPrefab.Length];

      for (int i = 0; i < impactPrefab.Length; ++i)
      {
          memoryPool[i] = new MemoryPool(impactPrefab[i]);
      }
   }

   public void SpawnImpact(RaycastHit hit)
   {
       // 부딪힌 오브젝트의 tag 정보에 따라 처리
       if (hit.transform.CompareTag("ImpactNormal"))
       {
           OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
       }
       else if (hit.transform.CompareTag("ImpactObstacle"))
       {
           OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
       }
   }

   public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation)
   {
       var item = memoryPool[(int)type].ActivatePoolItem();
       item.transform.position = position;
       item.transform.rotation = rotation;
       
       item.GetComponent<Impact>().Setup(memoryPool[(int)type]);
   }
   
}
