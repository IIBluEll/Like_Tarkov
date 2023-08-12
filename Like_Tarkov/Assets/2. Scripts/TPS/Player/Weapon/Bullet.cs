using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float deactiveTime = 1f;
    [SerializeField] private float ricochetForce = 10f; // 도탄 힘
    [SerializeField] private float minRicochetAngle = 30f; // 도탄이 일어나는 최소 각도
    
    private Rigidbody rigidbody;
    private MemoryPool memoryPool;

    private  int ricochetCount = 1;

    public void SetUp(MemoryPool pool, Vector3 direction)
    {
        rigidbody = GetComponent<Rigidbody>();
        memoryPool = pool;
        
        rigidbody.AddForce(direction * 1, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (ricochetCount > 0)
        {
            var incomingVector = rigidbody.velocity;
            var reflectVector = Vector3.Reflect(incomingVector, other.contacts[0].normal);
           
            // 충돌한 표면의 법선과 총알의 진행 방향 사이의 각도 계산
            var angle = Vector3.Angle(incomingVector, -other.contacts[0].normal);

            if (angle > minRicochetAngle)
            {
                Ricochet(reflectVector);
            }
            else
            {
                StartCoroutine("DeactiveAfterTime");
            }
        }
        else
        {
            StartCoroutine("DeactiveAfterTime");
        }
        //TODO : 탄이 부딪혔을 때 구현
    }
    
    private void Ricochet(Vector3 reflectVector)
    {
        // 총알의 회전을 반사 방향으로 설정
        transform.rotation = (Quaternion.LookRotation(reflectVector) * Quaternion.Euler(90, 0, 0));
        // 총알에 도탄 힘 적용
        rigidbody.AddForce(reflectVector.normalized * 1, ForceMode.Impulse);

        ricochetCount--;
    }

    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(deactiveTime);

        memoryPool.DeactivatePoolItem(this.gameObject);
    }
}
