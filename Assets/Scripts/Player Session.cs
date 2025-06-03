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

    // 아이디 설정 및 초기화
    public void SetPlayerId(string id)
    {
        playerData.playerId = id;
        playerData.score = 0;
        playerData.stage = 1;
    }

    // 점수 누적
    public void AddScore(int amount)
    {
        playerData.score += amount;
    }

    // 스테이지 갱신
    public void SetStage(int stage)
    {
        playerData.stage = stage;
    }

    // 랭킹 매니저에 저장
    public void SaveToRanking()
    {
        RankingManager.Instance.AddPlayerResult(
            playerData.playerId,
            playerData.score,
            playerData.stage
        );
    }
}