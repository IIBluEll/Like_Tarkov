using UnityEngine;

/// <summary>
/// 1. 총기의 '흔들림' 효과를 구현
/// 2. 마우스의 움직임과 Smooth, SwayMultiple 변수를 통해 회전 정도 조절 가능 
/// </summary>
public class GunSway : MonoBehaviour
{
    [Header(" Sway Setting")]
    [SerializeField] private float smooth;          // 부드러운 회전을 위함 
    [SerializeField] private float swayMulitple;    // 총기 흔들림 배수 조정

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMulitple;  // 마우스 X축 
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMulitple;  // 마우스 Y축

        // X축 및 Y축 회전 쿼터니언 계산
        var rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        var rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        // X축과 Y축 회전 쿼터니언 결합
        var targetRotation = rotationX * rotationY;
        
        // 총기 회전 ( 현재 회전에서 목표 회전까지 부드럽게 회전 )
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
