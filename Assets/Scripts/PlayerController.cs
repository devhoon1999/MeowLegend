using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Jump Settings")]
    public float fallMultiplier = 2.5f;    // ������ �� �߷� ��� (1���� ũ�� ������ ������)
    public float lowJumpMultiplier = 2f;   // ���� ��ư ª�� ������ �� �߷� ���

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

        // �̵� ó��
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = moveInput * moveSpeed;
        rb.velocity = newVelocity;

        // �¿� ����
        if (moveInput != 0)
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(moveInput), originalScale.y, originalScale.z);

        // �� üũ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ���� ó�� (�����̽��� ���� ������)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetTrigger("JumpTrigger");
        }

        // ���� �ӵ� ������ (�߰� �߷� ����)
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) // ���� �� ��ư ���� ��
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // �ִϸ��̼� ���� ����
        bool isWalking = moveInput != 0;
        animator.SetBool("isWalking", isWalking);

        // ī�޶� ������ �� ������ ����
        ClampToCameraBounds();
    }

    private void ClampToCameraBounds()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
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
