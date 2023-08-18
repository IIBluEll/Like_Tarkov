using System;
using UnityEngine;

public class New_CameraMove : MonoBehaviour
{
    [Header("Assign Objects")]
    // 카메라가 따라갈 대상
    [SerializeField] private Transform objectToFollow;
    // 실제 카메라 Transform
    [SerializeField] private Transform realCamera;
    // 플레이어 오브젝트
    [SerializeField] private Transform playerBody;
    // 입력 시스템
    [SerializeField] private PlayerInputMgr playerInputMgr;
    
    [Space(10f), Header("Camera Status")]
    [SerializeField] private float followSpeed = 10f;   // 카메라 이동 속도
    [SerializeField] private float sensitvity = 100f;   // 마우스 움직임에 따른 회전 감도
    [SerializeField] private float clamAngle = 50f;     // 
    
    [Space(10f), Header("Camera Distance Setting")]
    [SerializeField] private float minDistance;         // 최소 거리
    [SerializeField] private float maxDistance;         // 최대 거리
    [SerializeField] private float finalDistance;
    [SerializeField] private float smoothness = 10f;
    
    private float rotX;
    private float rotY;
    
    private Vector3 dirNormailzed;
    private Vector3 finalDir;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var transRot = transform.localRotation;
        rotX = transRot.eulerAngles.x;           // 초기 X축 회전값
        rotY = transRot.eulerAngles.y;           // 초기 Y축 회전값

        var cameraPos = realCamera.localPosition;
        dirNormailzed = cameraPos.normalized;    // 초기 카메라의 방향 벡터 설정
        finalDistance = cameraPos.magnitude;     // 초기 카메라와 플레이어 거리 설정
    }

    private void Update()
    {
        // 마우스의 움직임에 따라 카메라의 회전값을 변경
        rotX += -Input.GetAxis("Mouse Y") * sensitvity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitvity * Time.deltaTime;
        
        // 카메라의 회전값을 제한 각도 내로 고정
        rotX = Math.Clamp(rotX, -clamAngle, clamAngle);
        var rot = Quaternion.Euler(rotX, rotY, 0);

        // 카메라의 회전 적용
        transform.rotation = rot;
    }
    
    private void LateUpdate()
    {
        FreeFov();
        //PlayerBodyRotate();
    }
    
    private void FreeFov()
    {
        // 카메라가 플레이어를 따라가도록 위치 업데이트
        transform.position = Vector3.MoveTowards(transform.position, 
            objectToFollow.position, 
            followSpeed * Time.deltaTime);
        
        finalDir = transform.TransformPoint(dirNormailzed * maxDistance);

        RaycastHit hit;

        // 카메라와 플레이어 사이에 오브젝트가 있는지 확인
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            // if (hit.transform.CompareTag("Player"))
            //     return;
            
            Debug.Log($"Camera Hit Object! :: {hit.transform.name} ");
            
            // 오브젝트가 있다면, 카메라와 플레이어 간의 거리를 오브젝트까지의 거리로 설정
            finalDistance = Math.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            // 오브젝트가 없다면, 카메라와 플레이어간의 거리는 최대 거리로 설정
            finalDistance = maxDistance;
        }

        // 카메라의 위치를 부드럽게 이동
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, 
            dirNormailzed * finalDistance,
            Time.deltaTime * smoothness);
    }
    
    private void PlayerBodyRotate()
    {
        // 플레이어 몸이 카메라를 자연스럽게 따라가게 회전
        var playerRotate = Vector3.Scale(realCamera.transform.forward, new Vector3(1, 0, 1));
        playerBody.rotation = Quaternion.Slerp(playerBody.rotation,Quaternion.LookRotation(playerRotate),Time.deltaTime * smoothness );
        
    }
}
