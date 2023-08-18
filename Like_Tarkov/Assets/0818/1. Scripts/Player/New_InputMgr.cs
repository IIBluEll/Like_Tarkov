using System;
using UnityEngine;

public class New_InputMgr : MonoBehaviour
{
   private PlayerControl playerContorl;
   private New_PlayerAnimCtl animatorCtl;

   [Header("Player Movement")] 
   [SerializeField]  private float verticaMoveInput;
   [SerializeField]  private float horizontalMoveInput; 
                     public Vector2 movementInput;

   [Header("Camera Rotation")] 
   [SerializeField]  private float verticalCameraInput;
   [SerializeField]  private float horizontalCameraInput;
                     public Vector2 cameraInput;

   private void Awake()
   {
      animatorCtl = GetComponent<New_PlayerAnimCtl>();
   }

   private void OnEnable()
   {
      if (playerContorl == null)
      {
         playerContorl = new PlayerControl();

         playerContorl.PlayerMove.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
         playerContorl.PlayerMove.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
      }
      
      playerContorl.Enable();
   }

   private void OnDisable()
   {
      playerContorl.Disable();
   }
}
