using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum PopupType
{
    AudioSettings
    // �ʿ��� �˾� Ÿ�� �߰�
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
    [SerializeField] private GameObject blockerPrefab; // Ŭ�� ���ܿ� ���Ŀ ������

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
    /// �˾� �ݱ� (����) + ���Ŀ ���� ����
    /// �˾� ��ü�� �ݱ� ��ư�̳� ���Ŀ Ŭ���� �� �޼��� ȣ�� ��õ
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
    /// �˾� ���� �ı� (�ʿ� ��)
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
            // ���Ŀ�� ���� �ֻ������ �ø� (�ֻ�� ���)
            globalBlocker.transform.SetAsLastSibling();

            AddBlockerClickEvent(globalBlocker);
        }
        globalBlocker.SetActive(true);

        // ���Ŀ�� �ֻ�ܿ�, �˾��� �� ���� �ٽ� �ø�
        globalBlocker.transform.SetAsLastSibling();

        BringTopmostPopupToFront();
    }

    // ���Ŀ Ŭ�� �� ���� �� �˾� �ݱ� �̺�Ʈ �߰�
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

    // ���� ���� Ȱ��ȭ�� �˾��� ����
    private void CloseTopmostPopup()
    {
        // activePopups �� Ȱ��ȭ�� �͸� ���͸� ��, ������ ������ �˾��� ����
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
        // ���Ŀ�� �׻� �˾� �Ʒ��� �־�� �ϹǷ� ���� ���Ŀ�� �ø�
        if (globalBlocker != null)
        {
            globalBlocker.transform.SetAsLastSibling();
        }

        // �˾��� ���Ŀ ���� �ø�
        obj.transform.SetAsLastSibling();
    }

    // Ȱ��ȭ�� �˾� �� ���� �ֱ�(������)�� Ȱ��ȭ�� �˾��� �ֻ����� �ø�
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