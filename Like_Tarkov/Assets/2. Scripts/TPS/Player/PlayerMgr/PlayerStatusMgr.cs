using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusMgr : MonoBehaviour
{
    
    
    
    
    
    
    #region Stamina Status

    protected int max_stamina;            // 최대 스테미나 
    protected int current_Stamina;        // 현재 스테미나
    
    

    #endregion
    
    
    
    protected int walk_Speed;             // 걷기 속도
    protected int run_Speed;              // 질주 속도
    
    protected float avoid_Attack_Rate;    // 회피율
    
    protected float damage_magnification; // 데미지 증가 배율
    
    protected float skill_CoolTime_Down; // 스킬 쿨타임 감소율

    

}
