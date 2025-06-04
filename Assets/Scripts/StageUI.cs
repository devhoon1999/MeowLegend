using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stageText;

    private void Update()
    {
        UpdateStage();
    }

    public void UpdateStage()
    {
        int currentStage = PlayerSession.Instance.playerData.stage;
        stageText.text = $"{currentStage}";
    }
}
