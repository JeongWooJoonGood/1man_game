using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 5f;

    // 컴포넌트
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }
}