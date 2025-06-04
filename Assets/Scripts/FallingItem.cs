using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [SerializeField] private string groundLayerName = "Ground";
    [SerializeField] private string playerLayerName = "Player";

    private int groundLayer;
    private int playerLayer;

    private float fallSpeed = 3f; // �ʴ� �������� �Ÿ�

    private float groundY = 0f; // �� y ��ǥ (�⺻��)

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer(groundLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        // �� ������Ʈ ã�� (������ groundY �⺻�� 0 ����)
        GameObject ground = GameObject.FindWithTag("Ground");
        if (ground != null)
        {
            // �� ��ġ ���� y�� ����
            groundY = ground.transform.position.y;
        }
    }

    private void Update()
    {
        // ������ �Ʒ��� �̵�
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // ���� ������ ��Ȱ��ȭ ó�� (y ��ġ ��)
        if (transform.position.y <= groundY)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            if (gameObject.name.Contains("Skull"))
            {
                PlayerSession.Instance.MinusHeart();
                Debug.Log("�ƾ� ������!"); // test
            }
            else
            {
                PlayerSession.Instance.AddScore(100);
                Debug.Log("100�� �߰�!"); // test
            }
            gameObject.SetActive(false);
        }
    }
}
