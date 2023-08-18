using System.Collections;
using UnityEngine;

public class TPS_PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private TPS_AussultRifle aussultRifle;
    private PlayerInputMgr inputMgr;
    private PlayerMovement_Status movementStatus;
    private Rigidbody rb;
    private PlayerAnimationController playerAnimCtl;
    
    
    private float horizontalMove;
    private float verticalMove;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    
    [Header(" Movement ")] 
                     public float moveSpeed = 4f;
    [SerializeField] private float movementMulti = 10f;     // 움직임 배율
    [SerializeField] private float airMulti = 0.4f;         // 공중 움직임 배율
    
    [Space(10f), Header(" Sprinting ")]
    [SerializeField] private float acceleration = 10f;      // 가속도

    [Space(10f), Header(" Drag ")] 
    [SerializeField] private float groundDrag = 6f;         // 지면 마찰력
    [SerializeField] private float airDrag = 2f;            // 공중 마찰력
    
    [Space(10f), Header(" Ground Detection")] 
    [SerializeField] private LayerMask groundMask;          // 바닥 감지 레이어 마스크
    [SerializeField] private Transform groundCheck;         // 바닥 감지 위치
    [SerializeField] private bool isGrounded;               // 바닥에 닿았는지?
                     private float groundDistance = 0.1f;   // 바닥과의 거리
                     private float playerHeight = 2f;       // 플레이어 높이

    
    private RaycastHit slopeHit;                            // 경사면 감지 정보
    
    private float smoothness = 10f;                         // 부드러운 움직임을 위한 배율
    
    private bool isAvoid = false;                           // 회피중인지?
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimCtl = GetComponent<PlayerAnimationController>();
        inputMgr = GetComponent<PlayerInputMgr>();
        movementStatus = GetComponent<PlayerMovement_Status>();
        
        // 회전을 고정시킴
        rb.freezeRotation = true;
        
        #if UNITY_EDITOR
        //Debug - 컴포넌트들이 할당되었는지 확인
        CheckAssigned();
        #endif
    }

    private void Update()
    {
        // 바닥 감지 확인
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        PlayerInput();      // 플레이어 입력을 처리하는 메서드
        ControlSpeed();     // 속도를 제어하는 메서드
        WeaponAction();     // 무기 발사 메서드

        // 경사면에서의 움직임 방향 계산
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();   // 플레이어 움직임을 처리하는 메서드
        
        // 플레이어 애니메이션 처리
        playerAnimCtl.AnimMoveSpeed = moveSpeed;
        playerAnimCtl.DirX = horizontalMove;
        playerAnimCtl.DirY = verticalMove;
    }

    // 경사면 위에 있는지 검사하는 메서드
    private bool OnSlope()
    {
        // 아래 방향으로 레이를 쏘아 경사면 감지를 시도함
        if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, 0.1f)) 
            return false;

        // 경사면의 normal 벡터가 up이 아니라면 경사면이라고 판단
        return slopeHit.normal != Vector3.up;
    }
    
    // 플레이어 입력을 처리하는 메서드
    private void PlayerInput()
    {
        horizontalMove = inputMgr.Horizontal;   // 수평
        verticalMove = inputMgr.Vertical;       // 수직

        moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
    }

    // 플레이어 속도 제어를 하는 메서드
    private void ControlSpeed()
    {
        // 달리기 상태이고 바닥에 있을 때 
        if (inputMgr.IsSprinting && isGrounded)
        {
            // Sprint 속도로 변경
            moveSpeed = Mathf.Lerp(moveSpeed, movementStatus.Run_Speed, acceleration * Time.deltaTime);
        }
        // 움직이지 않을 때
        else if (horizontalMove == 0 && verticalMove == 0)
        {
            // 속도 0으로 변경
            moveSpeed = Mathf.Lerp(moveSpeed, 0, acceleration * Time.deltaTime);
        }
        // 걷는 상태이고 바닥에 있을 때 
        else
        {
            // Walk 속도로 변경
            moveSpeed = Mathf.Lerp(moveSpeed, movementStatus.Walk_Speed, acceleration * Time.deltaTime);
        }
    }

    // 플레이어가 움직이는 메서드
    private void MovePlayer()
    {
        switch (isGrounded)
        {
            // 바닥에 닿고 있으며 경사면이 아닐 때
            case true when !OnSlope():
                rb.AddForce(moveDirection.normalized * moveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            // 바닥에 닿고 있으며 경사면일 때
            case true when OnSlope():
                rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            // 공중에 있을 때 
            case false:
                rb.AddForce(moveDirection.normalized * moveSpeed * movementMulti * airMulti, ForceMode.Acceleration);
                rb.drag = airDrag;
                break;
        }
    }
    
    private void WeaponAction()
    {
        if (inputMgr.Mouse0ButtonDown && inputMgr.Mouse1Button)
        {
            aussultRifle.StartWeaponAction();
        }
        else if (inputMgr.Mouse0ButtonUp)
        {
            aussultRifle.StopWeaponAction();
        }

        if (inputMgr.IsReload)
        {
            aussultRifle.StartReload();
        }

    }

    
    //--------Debug-----------//
    // Assigned이 제대로 되었는지 체크
    private void CheckAssigned()
    {
        Debug.Assert(inputMgr,"[TPS_PlayerMovement.cs] <inputMgr> is Not Assigned!!");
        Debug.Assert(movementStatus, "[TPS_PlayerMovement.cs] <movementStatus> is not Assigned!!");
        Debug.Assert(rb, "[TPS_PlayerMovement.cs] <rb> is not Assigned!!");
    }
}
