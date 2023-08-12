using UnityEngine;

public class PlayerInputMgr : MonoBehaviour
{
   public float Horizontal { get; private set; }
   public float Vertical { get; private set; }

   public bool IsSprinting { get; private set; }
   public bool IsAvoiding { get; private set; }
   public bool IsReload { get; private set; }
   
   public bool Mouse0ButtonDown { get; private set; }
   public bool Mouse0ButtonUp { get; private set; }
   public bool Mouse1Button { get; private set; }
   

   private void Update()
   {
      Horizontal = Input.GetAxis("Horizontal");
      Vertical = Input.GetAxis("Vertical");

      IsSprinting = Input.GetKey(KeyCode.LeftShift);
      IsReload = Input.GetKeyDown(KeyCode.R);
      IsAvoiding = Input.GetKeyDown(KeyCode.Space);

      Mouse0ButtonDown = Input.GetMouseButtonDown(0);
      Mouse0ButtonUp = Input.GetMouseButtonUp(0);
      Mouse1Button = Input.GetMouseButton(1);
   }
}
