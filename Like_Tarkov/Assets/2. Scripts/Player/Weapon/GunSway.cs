using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [Header(" Sway Setting")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMulitple;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMulitple;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMulitple;

        var rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        var targetRotation = rotationX * rotationY;
        
        // 총기 회전
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
