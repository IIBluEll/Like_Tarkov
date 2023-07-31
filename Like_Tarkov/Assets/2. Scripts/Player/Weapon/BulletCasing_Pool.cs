using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletCasing_Pool : MonoBehaviour
{
    [SerializeField] private GameObject casingPrefeb;

    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(casingPrefeb);
    }

    public void Spwancasing(Vector3 position, Vector3 direction)
    {
        GameObject item = memoryPool.ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = Random.rotation;
        
        item.GetComponent<BulletCasing>().SetUp(memoryPool,direction);
    }
}
