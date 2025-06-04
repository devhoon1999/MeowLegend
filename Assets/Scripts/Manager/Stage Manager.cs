using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stage Settings")]
    public int currentStage = 1;           // 현재 스테이지 번호
    public int maxStage = 5;               // 최대 스테이지 번호 (원하는대로 조절)
    public float intervalSeconds = 30f;    // 스테이지 변경 간격 (초)
    public FallingItemSpawner spawner;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= intervalSeconds)
        {
            timer = 0f;
            AdvanceStage();
        }
    }

    private void AdvanceStage()
    {
        if (currentStage < maxStage)
        {
            currentStage++;
            Debug.Log("Stage 변경! 현재 스테이지: " + currentStage);

            // 여기에 스테이지 변경 시 처리할 로직 추가
            // 예: 맵 리셋, 적 세트 변경, 난이도 조절 등
            OnStageChanged(currentStage);
        }
        else
        {
            Debug.Log("최대 스테이지 도달");
            // 필요시 최대 스테이지 도달 시 처리
        }
    }

    private void OnStageChanged(int stage)
    {
        PlayerSession.Instance.SetStage(stage);
        ReduceSpawnIntervalByPercent(80f);
    }

    public void ReduceSpawnIntervalByPercent(float percent)
    {
        percent = Mathf.Clamp(percent, 0f, 100f); // 0%~100% 사이 제한
        float multiplier = percent / 100f;
        spawner.spawnInterval *= multiplier;
    }
}
