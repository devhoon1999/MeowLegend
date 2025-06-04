using UnityEngine;

public class HeartUI : MonoBehaviour
{
    [SerializeField] private GameObject heart1;  // ���� ���� Ȥ�� ù��° ��Ʈ
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;

    private void Update()
    {
        UpdateHearts();
    }

    // PlayerSession ��Ʈ ���� �ٲ� ������ ȣ��
    public void UpdateHearts()
    {
        int currentHearts = PlayerSession.Instance.playerData.heart;

        heart1.SetActive(currentHearts >= 1);
        heart2.SetActive(currentHearts >= 2);
        heart3.SetActive(currentHearts >= 3);
    }
}