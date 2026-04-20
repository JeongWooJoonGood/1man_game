using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool isLocked = false;
    public Sprite openSprite;
    public Sprite closedSprite;

    private bool playerNearby = false;
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D doorCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();

        // 처음엔 닫힌 문
        if (closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }

        // 처음엔 통과 불가!
        doorCollider.isTrigger = false; // ← 중요!
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isLocked)
            {
                ToggleDoor();
            }
            else
            {
                Debug.Log("문이 잠겨있습니다!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("[E] 문 열기");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    // Collision도 감지 (Is Trigger OFF일 때)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            Debug.Log("[E] 문 열기");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            // 문 열기
            if (openSprite != null)
            {
                spriteRenderer.sprite = openSprite;
            }

            // Trigger로 변경 (통과 가능!)
            doorCollider.isTrigger = true;
            Debug.Log("문 열림");
        }
        else
        {
            // 문 닫기
            if (closedSprite != null)
            {
                spriteRenderer.sprite = closedSprite;
            }

            // Trigger 해제 (통과 불가!)
            doorCollider.isTrigger = false;
            Debug.Log("문 닫힘");
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("문 잠금 해제!");
    }
}