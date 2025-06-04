using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [SerializeField] private string groundLayerName = "Ground";
    [SerializeField] private string playerLayerName = "Player";

    private int groundLayer;
    private int playerLayer;

    private float fallSpeed = 3f; // 초당 떨어지는 거리

    private float groundY = 0f; // 땅 y 좌표 (기본값)

    private void Awake()
    {
        groundLayer = LayerMask.NameToLayer(groundLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        // 땅 오브젝트 찾기 (없으면 groundY 기본값 0 유지)
        GameObject ground = GameObject.FindWithTag("Ground");
        if (ground != null)
        {
            // 땅 위치 기준 y값 세팅
            groundY = ground.transform.position.y;
        }
    }

    private void Update()
    {
        // 아이템 아래로 이동
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 땅에 닿으면 비활성화 처리 (y 위치 비교)
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
                Debug.Log("아얏 아프다!"); // test
            }
            else
            {
                PlayerSession.Instance.AddScore(100);
                Debug.Log("100점 추가!"); // test
            }
            gameObject.SetActive(false);
        }
    }
}
