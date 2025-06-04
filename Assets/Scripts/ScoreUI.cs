using UnityEngine;
using TMPro;      

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private void Update()
    {
        if (PlayerSession.Instance != null)
        {
            int score = PlayerSession.Instance.playerData.score;
            scoreText.text = $"{score}";
        }
    }
}
