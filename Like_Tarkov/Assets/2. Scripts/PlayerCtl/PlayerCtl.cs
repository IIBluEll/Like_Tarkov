using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    private CamCtl camCtl;
    private PlayerMoveCtl playermove;
    private MoveStatus moveStatus;
    
    private void Awake()
    {
        camCtl = GetComponent<CamCtl>();
        playermove = GetComponent<PlayerMoveCtl>();
        moveStatus = GetComponent<MoveStatus>();
    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
    }

    private void UpdateRotate()
    {
        var mouseX = Input.GetAxis("Mouse X");
       var mouseY = Input.GetAxis("Mouse Y");
        
        camCtl.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            var isRun = false;
            
            if(z > 0)
                isRun = Input.GetKey(KeyCode.LeftShift);
            
            playermove.MoveSpeed = isRun ? moveStatus.RunSpeed : moveStatus.WalkSpeed;
        }
        
        playermove.MoveTo(new Vector3(x,0,z));
    }
    
}
