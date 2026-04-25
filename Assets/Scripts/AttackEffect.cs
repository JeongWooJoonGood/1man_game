using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [Header("Settings")]
    public float destroyDelay = 0.5f; // 애니메이션 길이와 맞춤

    void Start()
    {
        // 애니메이션 끝나면 자동 삭제
        Destroy(gameObject, destroyDelay);
    }
}