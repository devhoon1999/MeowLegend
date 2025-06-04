using UnityEngine;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private GameObject heart1;  // 제일 왼쪽 혹은 첫번째 하트
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;

    private void Update()
    {
        UpdateHearts();
    }

    // PlayerSession 하트 개수 바뀔 때마다 호출
    public void UpdateHearts()
    {
        int currentHearts = PlayerSession.Instance.playerData.heart;

        heart1.SetActive(currentHearts >= 1);
        heart2.SetActive(currentHearts >= 2);
        heart3.SetActive(currentHearts >= 3);
    }
}