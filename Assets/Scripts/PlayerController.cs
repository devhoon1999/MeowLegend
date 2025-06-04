using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private Camera mainCamera;
    private Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // 이동 처리
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = moveInput * moveSpeed;
        rb.velocity = newVelocity;

        // 좌우 반전
        if (moveInput != 0)
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(moveInput), originalScale.y, originalScale.z);

        // 땅 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 점프 처리 (스페이스바를 누른 순간만 트리거 발동)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("JumpTrigger");
        }

        // 애니메이션 상태 설정
        bool isWalking = moveInput != 0;
        animator.SetBool("isWalking", isWalking);

        // 카메라 밖으로 못 나가게 제한
        ClampToCameraBounds();
    }

    private void ClampToCameraBounds()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);  // 좌우 여유 범위
        Vector3 clampedWorldPos = mainCamera.ViewportToWorldPoint(viewPos);
        transform.position = new Vector3(clampedWorldPos.x, transform.position.y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
