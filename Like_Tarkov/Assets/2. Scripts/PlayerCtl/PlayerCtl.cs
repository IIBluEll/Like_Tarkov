using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtl : MonoBehaviour
{
    private CamCtl camCtl;
    private PlayerInput playerInput;
    
    private void Awake()
    {
        camCtl = GetComponent<CamCtl>();
    }

    private void Update()
    {
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        var mouseDelta = Mouse.current.delta.ReadValue();
        
       var mouseX = mouseDelta.x;
       var mouseY = mouseDelta.y;
        
        camCtl.UpdateRotate(mouseX, mouseY);
    }
    
}
