using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 따라갈 대상 (플레이어)
    public Transform target;
    // 따라가는 속도
    public float smoothSpeed = 0.125f;
    // 카메라 위치 오프셋
    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        if (target != null)
        {
            // 목표 위치
            Vector3 desiredPosition = target.position + offset;
            // 부드럽게 이동
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}