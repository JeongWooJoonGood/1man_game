using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float attackRange = 1f;
    public float attackCooldown = 0.5f;
    public float attackDuration = 0.3f;
    public float swingAngle = 90f;

    [Header("Position Settings")]
    public Vector2 rightPosition = new Vector2(0.3f, 0f);
    public Vector2 leftPosition = new Vector2(-0.3f, 0f);

    [Header("References")]
    public GameObject attackEffect;

    private float lastAttackTime;
    private PlayerController playerController;
    private bool isAttacking = false;
    private float attackStartTime;
    private float startRotation;
    private float targetRotation;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();

        // 이펙트는 선택사항 (있으면 사용)
        if (attackEffect != null)
        {
            attackEffect.SetActive(false);
        }
    }

    void Update()
    {
        // 공격 입력
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            TryAttack();
        }

        // 공격 애니메이션 처리
        if (isAttacking)
        {
            UpdateAttackAnimation();
        }
        else
        {
            // 평상시 무기 위치/회전 업데이트
            UpdateWeaponTransform();
        }
    }

    void TryAttack()
    {
        // 쿨다운 체크
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        // 공격 시작
        StartAttack();
        lastAttackTime = Time.time;
    }

    void StartAttack()
    {
        isAttacking = true;
        attackStartTime = Time.time;

        // 회전 각도 설정
        if (playerController.IsFacingRight())
        {
            startRotation = 0f;
            targetRotation = -90f;
        }
        else
        {
            startRotation = 0f;
            targetRotation = 90f;
        }

        // 이펙트 표시 (선택사항)
        if (attackEffect != null)
        {
            attackEffect.SetActive(true);
        }

        // 데미지 처리
        DealDamage();

        Debug.Log("공격!");
    }

    void UpdateAttackAnimation()
    {
        float elapsed = Time.time - attackStartTime;
        float progress = elapsed / attackDuration;

        if (progress >= 1f)
        {
            // 공격 종료
            isAttacking = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (attackEffect != null)
            {
                attackEffect.SetActive(false);
            }
            return;
        }

        // 회전 애니메이션
        float currentRotation = Mathf.Lerp(startRotation, targetRotation, progress);
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }

    void UpdateWeaponTransform()
    {
        if (playerController == null) return;

        // 플레이어 방향에 따라 위치 변경
        if (playerController.IsFacingRight())
        {
            transform.localPosition = rightPosition;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localPosition = leftPosition;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // 평상시 회전 초기화
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void DealDamage()
    {
        // 공격 범위 내 적 탐지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            // 적 태그 확인 (나중에 Enemy 스크립트로 변경)
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log("적 공격: " + hit.name);

                // 적 체력 감소 (Enemy 스크립트 있을 때)
                // Enemy enemy = hit.GetComponent<Enemy>();
                // if (enemy != null)
                // {
                //     enemy.TakeDamage(10);
                // }
            }
        }
    }

    // 디버그용: 공격 범위 시각화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}