using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float lifetime = 0.2f;  // 이펙트 지속 시간

    void OnEnable()
    {
        // 활성화되면 일정 시간 후 비활성화
        Invoke("Disable", lifetime);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}