using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStatus : MonoBehaviour
{
    [Header("Player Movement Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
}