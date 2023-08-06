using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Status : MonoBehaviour
{
    [SerializeField] private int walk_Speed = 4;             // 걷기 속도
    [SerializeField] private int run_Speed = 6;              // 질주 속도

    public int Walk_Speed
    {
        get => walk_Speed;
        set
        {
            int changeSpeed = value - walk_Speed;
            walk_Speed = value;

            Run_Speed += changeSpeed;
        }
    }

    public int Run_Speed
    {
        get => run_Speed;
        set
        {
            run_Speed = value;
        }
    }

}
