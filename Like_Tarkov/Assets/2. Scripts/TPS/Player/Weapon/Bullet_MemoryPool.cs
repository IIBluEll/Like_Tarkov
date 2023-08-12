using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_MemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefeb;
    
    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(bulletPrefeb);
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation, Vector3 attackDirection)
    {
        var bullet = memoryPool.ActivatePoolItem();
        
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;

        bullet.GetComponent<Bullet>().SetUp(memoryPool, attackDirection);
    }
    
}
