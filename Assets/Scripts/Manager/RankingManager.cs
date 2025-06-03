using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }

    private string filePath;
    private RankingData rankingData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
            LoadRanking();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadRanking()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            rankingData = JsonUtility.FromJson<RankingData>(json);
        }
        else
        {
            rankingData = new RankingData();
        }
    }

    private void SaveRanking()
    {
        string json = JsonUtility.ToJson(rankingData, true);
        File.WriteAllText(filePath, json);
    }

    public bool IsIdAlreadyUsed(string playerId)
    {
        return rankingData.players.Exists(p => p.playerId == playerId);
    }

    public void AddPlayerResult(string playerId, int score, int stage)
    {
        if (IsIdAlreadyUsed(playerId))
        {
            Debug.LogWarning("ID already used: " + playerId);
            return;
        }

        rankingData.players.Add(new PlayerData
        {
            playerId = playerId,
            score = score,
            stage = stage
        });

        SaveRanking();
    }

    public List<PlayerData> GetSortedRanking()
    {
        var sorted = new List<PlayerData>(rankingData.players);
        sorted.Sort((a, b) =>
        {
            int stageCompare = b.stage.CompareTo(a.stage); // 스테이지 내림차순
            if (stageCompare != 0) return stageCompare;
            return b.score.CompareTo(a.score);             // 점수 내림차순
        });

        return sorted;
    }

    public void ClearAllData()  // 필요 시 전체 초기화용
    {
        rankingData.players.Clear();
        SaveRanking();
    }
}