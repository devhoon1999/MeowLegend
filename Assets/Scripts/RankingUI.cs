using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RankingUI : MonoBehaviour
{
    [Header("���� 5�� ��ŷ �ؽ�Ʈ (����/���̵�/��������/���� ����)")]
    [SerializeField] private TMP_Text[] idTexts = new TMP_Text[5];
    [SerializeField] private TMP_Text[] stageTexts = new TMP_Text[5];
    [SerializeField] private TMP_Text[] scoreTexts = new TMP_Text[5];

    [Header("�� ���� ���� ǥ�ÿ�")]
    [SerializeField] private TMP_Text yourRankText;
    [SerializeField] private TMP_Text yourIdText;
    [SerializeField] private TMP_Text yourStageText;
    [SerializeField] private TMP_Text yourScoreText;
    [SerializeField] private GameObject yourRankContainer;

    private void Start()
    {
        PlayerSession.Instance.SaveToRanking();
        DisplayRanking();
    }

    public void DisplayRanking()
    {
        string currentPlayerId = PlayerSession.Instance.playerData.playerId;
        List<PlayerData> ranking = RankingManager.Instance.GetSortedRanking();

        bool isPlayerInTop = false;
        int yourRank = -1;

        // ���� 5�� ǥ��
        for (int i = 0; i < 5; i++)
        {
            if (i < ranking.Count)
            {
                PlayerData p = ranking[i];

                idTexts[i].text = p.playerId;
                stageTexts[i].text = $"Stage {p.stage}";
                scoreTexts[i].text = $"{p.score} pts";

                if (p.playerId == currentPlayerId)
                {
                    isPlayerInTop = true;
                }
            }
            else
            {
                idTexts[i].text = "-";
                stageTexts[i].text = "-";
                scoreTexts[i].text = "-";
            }
        }

        // �� ������ ���� 5�� �ȿ� ���� ��� ���� ǥ��
        if (!isPlayerInTop)
        {
            for (int i = 0; i < ranking.Count; i++)
            {
                if (ranking[i].playerId == currentPlayerId)
                {
                    yourRank = i + 1;
                    break;
                }
            }

            if (yourRank != -1)
            {
                PlayerData me = ranking[yourRank - 1];
                yourRankContainer.SetActive(true);
                yourRankText.text = yourRank.ToString() + "��";
                yourIdText.text = me.playerId;
                yourStageText.text = $"Stage {me.stage}";
                yourScoreText.text = $"{me.score} pts";
            }
            else
            {
                yourRankContainer.SetActive(false);
            }
        }
        else
        {
            yourRankContainer.SetActive(false);
        }
    }
}