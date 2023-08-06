using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHP_Status : MonoBehaviour
{
    private int max_HP = 100;                 // 최대 체력
    private int current_HP;             // 현재 체력
    private int avoid_Attack_Rate = 1;    // 회피율

    #region Property

    public int Current_HP
    {
        get => current_HP;
        set
        {
            current_HP = value;

            if (current_HP <= 0)
            {
                current_HP = 0;

                if (!isDead)
                {
                    isDead = true;
                    //TODO : 플레이어 사망 처리
                }
            }
            else if (current_HP > max_HP)
            {
                current_HP = max_HP;
            }
            
            //TODO : HP 변경 이벤트
        }
    }
    
    public int Max_HP
    {
        get => max_HP;
        set
        {
            // 최대 체력이 늘어나면 증감량만큼 현재 체력 증가
            int changeAmount = Math.Max(value - max_HP, 0);
            max_HP = value;

            Current_HP += changeAmount;
            
            Debug.Assert(max_HP > 0, "Error-Max HP is lower then 0");
            
            //TODO : HP 변경 이벤트
        }
    }

    public int Avoid_Attack_Rate { get; set; }

    #endregion

    private bool isDead = false; 

    private void OnEnable()
    {
        current_HP = max_HP;
        isDead = false;
    }

    private void Start()
    {
        // UI 갱신
        Current_HP = Current_HP;
    }

    public void TakeDamage(int amount)
    {
        if (AvoidAttack())
        {
            return;
        }
        
        Current_HP -= amount;

        if (!isDead)
        {
            //Todo : 데미지를 받았을 때
        }
    }

    public void TakeHeal(int amount)
    {
        Current_HP += amount;

        if (!isDead)
        {
            //TOdo : 힐했을 때
        }
    }

    private bool AvoidAttack()
    {
        int rate = Random.Range(1, 101);

        return Avoid_Attack_Rate > rate;
    }
}
