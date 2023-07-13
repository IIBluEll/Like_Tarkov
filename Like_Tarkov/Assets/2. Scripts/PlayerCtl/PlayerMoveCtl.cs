using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
class PlayerMoveCtl : MonoBehaviour
{
     [SerializeField]
     private float moveSpeed; // 이동 속도
     private Vector3 moveForce;   // 이동할 때 힘

     private CharacterController characterController;

     public float MoveSpeed
     {
          set => moveSpeed = Mathf.Max(0, value);
          get => moveSpeed;
     }

     private void Awake()
     {
          characterController = GetComponent<CharacterController>();
     }

     private void Update()
     {
          characterController.Move(moveForce * Time.deltaTime);
     }

     public void MoveTo(Vector3 direction)
     {
          direction = transform.rotation * new Vector3(direction.x, 0, direction.z);
          
          moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
     }
}