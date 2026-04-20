using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 컴포넌트 누락 체크
        if (animator == null)
            Debug.LogError("Animator가 없습니다!");
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer가 없습니다!");
    }

    void Update()
    {
        // 상하좌우 입력 받기
        float moveX = 0f;
        float moveY = 0f;

        // 좌우 이동 (A/D 또는 ←/→)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }

        // 상하 이동 (W/S 또는 ↑/↓)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }

        // 이동 벡터 생성
        Vector2 movement = new Vector2(moveX, moveY);

        // 대각선 이동 시 속도 정규화
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // 이동 적용
        rb.linearVelocity = movement * moveSpeed;

        // 애니메이션 업데이트
        UpdateAnimation(movement);

        // 좌우 반전
        FlipSprite(moveX);
    }

    void UpdateAnimation(Vector2 movement)
    {
        if (animator == null) return;

        // 움직이는지 체크
        if (movement != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void FlipSprite(float moveX)
    {
        if (spriteRenderer == null) return;

        // 좌우 반전
        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    // 무기 시스템용 함수
    public bool IsFacingRight()
    {
        if (spriteRenderer == null) return true;
        return !spriteRenderer.flipX;
    }
}