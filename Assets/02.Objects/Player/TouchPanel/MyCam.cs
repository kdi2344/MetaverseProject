using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyCam : MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;
    public Transform target;
    [SerializeField] private float rotSensitive = 3f;
    [SerializeField] private float dis = 2f;
    [SerializeField] private float RotationMin = -10f;
    [SerializeField] private float RotationMax = 80f;
    [SerializeField] private float smoothTime = 0.12f;
    private Vector3 targetRotation;
    private Vector3 currentVel;
    public bool enableMobile = false;
    public FixedTouchField touchField;
    void Start()
{
    // 모바일 플랫폼에서 자동 활성화
    enableMobile = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
}

    void LateUpdate(){
        if (touchField.Pressed) // 터치 중일 때만 회전 처리
        {
            if (enableMobile)
            {
                Yaxis += touchField.TouchDist.x * rotSensitive;
                Xaxis -= touchField.TouchDist.y * rotSensitive;
            }
            else
            {
                Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
                Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;
            }

            // X축 회전 제한 (최소~최대)
            Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

            // 회전 값 계산 및 적용
            targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
            transform.eulerAngles = targetRotation;
        }

        // 카메라의 위치를 타겟 기준으로 업데이트
        transform.position = target.position - transform.forward * dis + new Vector3(0, 1, 0);
    }
}
