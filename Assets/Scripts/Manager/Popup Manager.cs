using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum PopupType
{
    AudioSettings,
    PauseMenu,
    Giveup,
    IDInputfield,
    Ranking
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
    [SerializeField] private GameObject blockerPrefab;

    private Dictionary<PopupType, GameObject> activePopups = new Dictionary<PopupType, GameObject>();

    private GameObject globalBlocker;
    private Canvas currentCanvas;

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

    public void SetCanvas(Canvas canvas)
    {
        currentCanvas = canvas;
    }

    public void ShowPopup(PopupType type)
    {
        if (currentCanvas == null)
        {
            Debug.LogError("PopupManager: Canvas not assigned. Use SetCanvas() first.");
            return;
        }

        // 기존 팝업이 존재하면 파괴
        if (activePopups.TryGetValue(type, out GameObject existingPopup))
        {
            if (existingPopup != null)
            {
                Destroy(existingPopup);
            }
            activePopups.Remove(type);
        }

        var entry = popupPrefabs.Find(p => p.type == type);
        if (entry != null && entry.prefab != null)
        {
            GameObject popupInstance = Instantiate(entry.prefab, currentCanvas.transform, false);
            popupInstance.name = type.ToString(); // 디버깅 편하게 이름 설정
            activePopups[type] = popupInstance;

            // 다른 팝업들을 아래로 내림
            foreach (var popup in activePopups.Values)
            {
                if (popup != null && popup != popupInstance)
                {
                    popup.transform.SetSiblingIndex(0);
                }
            }

            BringToFront(popupInstance);
            ActivateBlocker();
        }
        else
        {
            Debug.LogWarning($"Popup prefab for type {type} not found.");
        }

        Time.timeScale = 0f;
    }

    public void ClosePopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup) && popup != null)
        {
            popup.SetActive(false);
            CheckBlockerStatus();
        }

        Time.timeScale = 1f;
    }

    public void DestroyPopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup) && popup != null)
        {
            Destroy(popup);
            activePopups.Remove(type);
            CheckBlockerStatus();
        }
    }

    private void ActivateBlocker()
    {
        if (currentCanvas == null)
        {
            Debug.LogError("PopupManager: Canvas not assigned. Cannot create blocker.");
            return;
        }

        if (globalBlocker == null)
        {
            if (blockerPrefab == null)
            {
                Debug.LogError("PopupManager: Blocker Prefab is not assigned!");
                return;
            }
            globalBlocker = Instantiate(blockerPrefab, currentCanvas.transform, false);
            globalBlocker.name = "Blocker";
            AddBlockerClickEvent(globalBlocker);
        }

        globalBlocker.SetActive(true);
        globalBlocker.transform.SetAsLastSibling();
        BringTopmostPopupToFront();
    }

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

    private void CloseTopmostPopup()
    {
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
            if (popup != null && popup.activeSelf)
            {
                anyActive = true;
                break;
            }
        }

        if (!anyActive && globalBlocker != null)
        {
            globalBlocker.SetActive(false);
        }
        else if (globalBlocker != null)
        {
            globalBlocker.transform.SetAsLastSibling();
            BringTopmostPopupToFront();
        }
    }

    private void BringToFront(GameObject obj)
    {
        if (globalBlocker != null)
        {
            globalBlocker.transform.SetAsLastSibling();
        }

        obj.transform.SetAsLastSibling();
    }

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
