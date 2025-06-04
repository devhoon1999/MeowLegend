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
        playerData.heart = 3;
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

    public void MinusHeart()
    {
        playerData.heart -= 1;
        if (playerData.heart <= 0)
        {
            playerData.heart = 0;  // 음수 방지

            // 게임 종료 처리 호출
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        Debug.Log("게임 종료! 하트가 모두 소진됨.");
        MySceneManager.Instance.LoadScene("Title");
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