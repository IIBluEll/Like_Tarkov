using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtl : MonoBehaviour
{
    [SerializeField] private float xAxisSpeed = 5; // x축 회전 속도
    [SerializeField] private float yAxisSpeed = 3; // y축 회전 속도

    [SerializeField] private float limitMinX = -80; // x축 회전 제한 최소값
    [SerializeField] private float limitMaxX = 80; // x축 회전 제한 최대값

    private float eulerAngleX;
    private float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * yAxisSpeed * Time.deltaTime; // 마우스 좌/우 이동 -> y축 회전
        eulerAngleX -= mouseY * xAxisSpeed * Time.deltaTime; // 마우스 상/하 이동 -> x축 회전

        // x축 회전 제한
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}