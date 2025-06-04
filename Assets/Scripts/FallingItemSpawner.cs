using System.Collections.Generic;
using UnityEngine;

public enum FallingItemType { Diamond, Saphire, Ruby, Emerald } // 풀 enum

[System.Serializable]
public class FallingItemEntry
{
    public FallingItemType type;
    public GameObject prefab;
    public int poolSize = 10;
}

public class FallingItemSpawner : MonoBehaviour
{
    public List<FallingItemEntry> itemEntries;
    public float spawnInterval = 1f;
    public float fallSpeed = 3f;  // 떨어지는 속도 (y축 이동 속도)

    private Dictionary<FallingItemType, Queue<GameObject>> objectPools = new();
    private Camera mainCamera;
    private float timer;

    private List<(FallingItemType type, float lastX)> recentSpawns = new();
    public float xSpacingThreshold = 1f; // 겹치지 않게 할 최소 간격
    public float xCooldown = 1f;         // 같은 위치로 다시 생성되지 않게 할 시간 제한

    private void Awake()
    {
        mainCamera = Camera.main;

        foreach (var entry in itemEntries)
        {
            Queue<GameObject> pool = new();
            for (int i = 0; i < entry.poolSize; i++)
            {
                GameObject obj = Instantiate(entry.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }

            objectPools[entry.type] = pool;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnRandomItem();
        }

        // 오래된 스폰 기록 정리
        for (int i = recentSpawns.Count - 1; i >= 0; i--)
        {
            if (Time.time - recentSpawns[i].lastX >= xCooldown)
                recentSpawns.RemoveAt(i);
        }
    }

    private void SpawnRandomItem()
    {
        FallingItemType type = GetRandomType();
        GameObject obj = GetPooledObject(type);
        if (obj == null) return;

        Vector3 spawnPos = GetNonOverlappingSpawnPosition();
        obj.transform.position = spawnPos;
        obj.SetActive(true);

        // 떨어지는 속도 세팅
        var fallingItem = obj.GetComponent<FallingItem>();
        if (fallingItem != null)
        {
            fallingItem.SetFallSpeed(fallSpeed);
        }

        recentSpawns.Add((type, spawnPos.x));
    }

    private FallingItemType GetRandomType()
    {
        int index = Random.Range(0, itemEntries.Count);
        return itemEntries[index].type;
    }

    private GameObject GetPooledObject(FallingItemType type)
    {
        if (!objectPools.TryGetValue(type, out var pool)) return null;

        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        return null; // 모두 사용 중일 경우 생성 안 함
    }

    private Vector3 GetNonOverlappingSpawnPosition()
    {
        float halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float minX = mainCamera.transform.position.x - halfWidth + 0.5f;
        float maxX = mainCamera.transform.position.x + halfWidth - 0.5f;

        float y = mainCamera.transform.position.y + mainCamera.orthographicSize + 1f;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            float x = Random.Range(minX, maxX);
            bool tooClose = false;

            foreach (var (type, recentX) in recentSpawns)
            {
                if (Mathf.Abs(x - recentX) < xSpacingThreshold)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return new Vector3(x, y, 0f);
        }

        return new Vector3(Random.Range(minX, maxX), y, 0f); // fallback
    }
}
