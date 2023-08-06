using UnityEngine;

[RequireComponent(typeof(Rigidbody),
                 typeof(PlayerInputMgr),
                 typeof(PlayerMovement_Status))]
public class TPS_PlayerMovement : MonoBehaviour
{
    private PlayerInputMgr inputMgr;
    private PlayerMovement_Status movementStatus;

    [SerializeField] private PlayerAnimationController playerAnimCtl;
    
    private float horizontalMove;
    private float verticalMove;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;
    
    [Header(" Movement ")] 
                     public float moveSpeed = 4f;
    [SerializeField] private float movementMulti = 10f;
    [SerializeField] private float airMulti = 0.4f;
    
    [Space(10f), Header(" Sprinting ")]
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
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        inputMgr = GetComponent<PlayerInputMgr>();
        movementStatus = GetComponent<PlayerMovement_Status>();
        
        //Debug
        CheckAssigned();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        PlayerInput();
        ControlSpeed();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        
        // Player Animation
        playerAnimCtl.AnimMoveSpeed = moveSpeed;
        playerAnimCtl.DirX = horizontalMove;
        playerAnimCtl.DirY = verticalMove;
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

        moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
    }

    private void ControlSpeed()
    {
        if (inputMgr.IsSprinting && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, movementStatus.Run_Speed, acceleration * Time.deltaTime);
        }
        else if (horizontalMove == 0 && verticalMove == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, movementStatus.Walk_Speed, acceleration * Time.deltaTime);
        }
    }

    private void MovePlayer()
    {
        switch (isGrounded)
        {
            case true when !OnSlope():
                rb.AddForce(moveDirection.normalized * moveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            case true when OnSlope():
                rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMulti, ForceMode.Acceleration);
                rb.drag = groundDrag;
                break;
            
            case false:
                rb.AddForce(moveDirection.normalized * moveSpeed * movementMulti * airMulti, ForceMode.Acceleration);
                rb.drag = airDrag;
                break;
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
