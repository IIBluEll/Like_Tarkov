using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TPS_PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInputMgr inputMgr;

    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform mainCamera;
    private float horizontalMove;
    private float verticalMove;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    
    [Header(" Movement ")] 
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float movementMulti = 10f;
    [SerializeField] private float airMulti = 0.4f;
    
    [Space(10f), Header(" Sprinting ")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float acceleration = 10f;

    [Space(10f), Header(" Drag ")] 
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;
    
    [Space(10f), Header(" Ground Detection")] 
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    private float groundDistance = 0.4f;
    private float playerHeight = 1.5f;

    private Rigidbody rb;
    private RaycastHit slopeHit;

    private float smoothness = 10f;
    
    public float MoveSpeed { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        PlayerInput();
        ControlSpeed();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void LateUpdate()
    {
        var playerRotate = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate),Time.deltaTime * smoothness);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private bool OnSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f)) 
            return false;

        return slopeHit.normal != Vector3.up;
    }
    
    private void PlayerInput()
    {
        horizontalMove = inputMgr.Horizontal;
        verticalMove = inputMgr.Vertical;

        moveDirection = playerBody.forward * verticalMove + playerBody.right * horizontalMove;
    }

    private void ControlSpeed()
    {
        if (inputMgr.IsSprinting && isGrounded)
        {
            MoveSpeed = Mathf.Lerp(MoveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else if (horizontalMove == 0 && verticalMove == 0)
        {
            MoveSpeed = Mathf.Lerp(MoveSpeed, 0, acceleration * Time.deltaTime);
        }
        else
        {
            MoveSpeed = Mathf.Lerp(MoveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    private void MovePlayer()
    {
        switch (isGrounded)
        {
            case true when !OnSlope():
                rb.AddForce(moveDirection.normalized * MoveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            case true when OnSlope():
                rb.AddForce(slopeMoveDirection.normalized * MoveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            case false:
                rb.AddForce(moveDirection.normalized * MoveSpeed * movementMulti * airMulti, ForceMode.Acceleration);
                rb.drag = airDrag;
                break;
        }
    }
}
