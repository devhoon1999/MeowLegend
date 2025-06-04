using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Stage Settings")]
    public int currentStage = 1;           // ���� �������� ��ȣ
    public int maxStage = 5;               // �ִ� �������� ��ȣ (���ϴ´�� ����)
    public float intervalSeconds = 30f;    // �������� ���� ���� (��)
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
            Debug.Log("Stage ����! ���� ��������: " + currentStage);

            // ���⿡ �������� ���� �� ó���� ���� �߰�
            // ��: �� ����, �� ��Ʈ ����, ���̵� ���� ��
            OnStageChanged(currentStage);
        }
        else
        {
            Debug.Log("�ִ� �������� ����");
            // �ʿ�� �ִ� �������� ���� �� ó��
        }
    }

    private void OnStageChanged(int stage)
    {
        PlayerSession.Instance.SetStage(stage);
        spawner.spawnInterval -= 0.05f;
    }
}
