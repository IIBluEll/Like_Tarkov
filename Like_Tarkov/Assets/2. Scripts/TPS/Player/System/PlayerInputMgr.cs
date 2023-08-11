using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMgr : MonoBehaviour
{
   public float Horizontal { get; private set; }
   public float Vertical { get; private set; }

   public bool IsJumping { get; private set; }
   public bool IsSprinting { get; private set; }
   public bool Mouse0ButtonDown { get; private set; }
   public bool Mouse0ButtonUp { get; private set; }
   
   public bool Mouse1Button { get; private set; }
   
   public bool IsReload { get; private set; }

   private void Update()
   {
      Horizontal = Input.GetAxis("Horizontal");
      Vertical = Input.GetAxis("Vertical");

      IsJumping = Input.GetKeyDown(KeyCode.Space);
      IsSprinting = Input.GetKey(KeyCode.LeftShift);
      IsReload = Input.GetKeyDown(KeyCode.R);

      Mouse0ButtonDown = Input.GetMouseButtonDown(0);
      Mouse0ButtonUp = Input.GetMouseButtonUp(0);

      Mouse1Button = Input.GetMouseButton(1);
   }
}
