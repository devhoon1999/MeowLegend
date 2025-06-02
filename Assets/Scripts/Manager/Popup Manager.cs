using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum PopupType
{
    AudioSettings
    // 필요한 팝업 타입 추가
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [System.Serializable]
    private class PopupEntry
    {
        public PopupType type;
        public GameObject prefab;
    }

    [Header("Popup Prefabs")]
    [SerializeField] private List<PopupEntry> popupPrefabs;

    [Header("Blocker Prefab")]
    [SerializeField] private GameObject blockerPrefab; // 클릭 차단용 블로커 프리팹

    [Header("Canvas Reference")]
    [SerializeField] private Canvas mainCanvas;

    private Dictionary<PopupType, GameObject> activePopups = new Dictionary<PopupType, GameObject>();

    private GameObject globalBlocker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (mainCanvas == null)
            {
                Debug.LogError("PopupManager: Main Canvas is not assigned!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopup(PopupType type)
    {
        if (activePopups.ContainsKey(type))
        {
            activePopups[type].SetActive(true);
            BringToFront(activePopups[type]);
            ActivateBlocker();
            return;
        }

        var entry = popupPrefabs.Find(p => p.type == type);
        if (entry != null && entry.prefab != null)
        {
            GameObject popupInstance = Instantiate(entry.prefab, mainCanvas.transform, false);
            activePopups[type] = popupInstance;
            BringToFront(popupInstance);
            ActivateBlocker();
        }
        else
        {
            Debug.LogWarning($"Popup prefab for type {type} not found.");
        }
    }

    /// <summary>
    /// 팝업 닫기 (숨김) + 블로커 상태 갱신
    /// 팝업 자체의 닫기 버튼이나 블로커 클릭시 이 메서드 호출 추천
    /// </summary>
    public void ClosePopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup))
        {
            popup.SetActive(false);
            CheckBlockerStatus();
        }
    }

    /// <summary>
    /// 팝업 완전 파괴 (필요 시)
    /// </summary>
    public void DestroyPopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup))
        {
            Destroy(popup);
            activePopups.Remove(type);
            CheckBlockerStatus();
        }
    }

    private void ActivateBlocker()
    {
        if (globalBlocker == null)
        {
            if (blockerPrefab == null)
            {
                Debug.LogError("PopupManager: Blocker Prefab is not assigned!");
                return;
            }
            globalBlocker = Instantiate(blockerPrefab, mainCanvas.transform, false);
            // 블로커를 먼저 최상단으로 올림 (최상단 배경)
            globalBlocker.transform.SetAsLastSibling();

            AddBlockerClickEvent(globalBlocker);
        }
        globalBlocker.SetActive(true);

        // 블로커는 최상단에, 팝업은 그 위에 다시 올림
        globalBlocker.transform.SetAsLastSibling();

        BringTopmostPopupToFront();
    }

    // 블로커 클릭 시 가장 위 팝업 닫기 이벤트 추가
    private void AddBlockerClickEvent(GameObject blocker)
    {
        var eventTrigger = blocker.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = blocker.AddComponent<EventTrigger>();
        }
        eventTrigger.triggers.Clear();

        var entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };
        entry.callback.AddListener((data) =>
        {
            CloseTopmostPopup();
        });
        eventTrigger.triggers.Add(entry);
    }

    // 가장 위에 활성화된 팝업을 닫음
    private void CloseTopmostPopup()
    {
        // activePopups 중 활성화된 것만 필터링 후, 마지막 생성된 팝업을 닫음
        for (int i = activePopups.Count - 1; i >= 0; i--)
        {
            var kvp = new List<KeyValuePair<PopupType, GameObject>>(activePopups)[i];
            if (kvp.Value != null && kvp.Value.activeSelf)
            {
                ClosePopup(kvp.Key);
                break;
            }
        }
    }

    private void CheckBlockerStatus()
    {
        bool anyActive = false;
        foreach (var popup in activePopups.Values)
        {
            if (popup.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        if (!anyActive && globalBlocker != null)
        {
            globalBlocker.SetActive(false);
        }
    }

    private void BringToFront(GameObject obj)
    {
        // 블로커는 항상 팝업 아래에 있어야 하므로 먼저 블로커를 올림
        if (globalBlocker != null)
        {
            globalBlocker.transform.SetAsLastSibling();
        }

        // 팝업을 블로커 위로 올림
        obj.transform.SetAsLastSibling();
    }

    // 활성화된 팝업 중 가장 최근(마지막)에 활성화된 팝업을 최상위로 올림
    private void BringTopmostPopupToFront()
    {
        for (int i = activePopups.Count - 1; i >= 0; i--)
        {
            var kvp = new List<KeyValuePair<PopupType, GameObject>>(activePopups)[i];
            if (kvp.Value != null && kvp.Value.activeSelf)
            {
                kvp.Value.transform.SetAsLastSibling();
                break;
            }
        }
    }
}