using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDebugDrawLine : MonoBehaviour
{
    private void Update()
    {
        Debug.DrawRay(transform.position,transform.forward * 100, Color.red);
    }
}
