using UnityEngine;

public class TornadoSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    public KeyCode skillKey = KeyCode.Q;
    public int requiredLevel = 30;
    public float cooldownTime = 5f;

    [Header("Tornado Settings")]
    public GameObject tornadoPrefab;
    public float tornadoDuration = 3f;
    public float tornadoRange = 3f;
    public int tornadoDamage = 10;
    public float damageInterval = 0.5f;
    public float maxCastRange = 5f;

    private ExperienceSystem experienceSystem;
    private float currentCooldown = 0f;
    private bool isOnCooldown = false;

    [Header("Cooldown Timer (Public)")]
    public float skillCooldownTimer = 0f;

    void Start()
    {
        experienceSystem = GetComponent<ExperienceSystem>();
    }

    void Update()
    {
        // UI용 타이머 업데이트
        if (isOnCooldown)
        {
            currentCooldown -= Time.deltaTime;
            skillCooldownTimer = currentCooldown;

            if (currentCooldown <= 0f)
            {
                isOnCooldown = false;
                skillCooldownTimer = 0f;
            }
        }

        // 스킬 사용 가능 시 커서 표시
        if (Input.GetKey(skillKey) && !isOnCooldown)
        {
            ShowCastRange();
        }

        // 스킬 사용
        if (Input.GetKeyDown(skillKey))
        {
            TryUseTornado();
        }
    }

    void TryUseTornado()
    {
        // 레벨 체크
        if (experienceSystem != null && experienceSystem.currentLevel < requiredLevel)
        {
            Debug.Log("Tornado requires level " + requiredLevel + "!");
            return;
        }

        // 쿨타임 체크
        if (isOnCooldown)
        {
            Debug.Log("Cooldown: " + Mathf.Ceil(currentCooldown) + "s");
            return;
        }

        // 스킬 발동
        UseTornado();
    }

    void UseTornado()
    {
        Debug.Log("TORNADO!");

        if (tornadoPrefab != null)
        {
            // 마우스 위치 가져오기
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            // 플레이어와의 거리 계산
            Vector3 direction = (mousePos - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, mousePos);

            // 최대 사거리 제한
            if (distance > maxCastRange)
            {
                distance = maxCastRange;
            }

            // 최종 생성 위치
            Vector3 spawnPosition = transform.position + direction * distance;

            // 회오리 생성
            GameObject tornado = Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);

            TornadoEffect effect = tornado.GetComponent<TornadoEffect>();
            if (effect != null)
            {
                effect.damage = tornadoDamage;
                effect.damageRange = tornadoRange;
                effect.damageInterval = damageInterval;
            }

            Destroy(tornado, tornadoDuration);
        }

        // 쿨타임 시작
        isOnCooldown = true;
        currentCooldown = cooldownTime;
        skillCooldownTimer = cooldownTime;
    }

    void ShowCastRange()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, mousePos);

        if (distance > maxCastRange)
        {
            distance = maxCastRange;
        }

        Vector3 targetPos = transform.position + direction * distance;

        // 플레이어에서 목표 지점까지 선
        Debug.DrawLine(transform.position, targetPos, Color.cyan);

        // 목표 지점에 십자 표시
        Debug.DrawLine(targetPos + Vector3.left * 0.3f, targetPos + Vector3.right * 0.3f, Color.yellow);
        Debug.DrawLine(targetPos + Vector3.up * 0.3f, targetPos + Vector3.down * 0.3f, Color.yellow);
    }

    void OnDrawGizmosSelected()
    {
        // 최대 사거리 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxCastRange);
    }
}