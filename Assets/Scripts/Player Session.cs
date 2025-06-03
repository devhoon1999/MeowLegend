using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    public static PlayerSession Instance { get; private set; }

    public PlayerData playerData { get; private set; } = new PlayerData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ���̵� ���� �� �ʱ�ȭ
    public void SetPlayerId(string id)
    {
        playerData.playerId = id;
        playerData.score = 0;
        playerData.stage = 1;
    }

    // ���� ����
    public void AddScore(int amount)
    {
        playerData.score += amount;
    }

    // �������� ����
    public void SetStage(int stage)
    {
        playerData.stage = stage;
    }

    // ��ŷ �Ŵ����� ����
    public void SaveToRanking()
    {
        RankingManager.Instance.AddPlayerResult(
            playerData.playerId,
            playerData.score,
            playerData.stage
        );
    }
}