using UnityEngine;
using System.Collections.Generic;

public enum PopupType
{
    AudioSettings
    // �ʿ��� �˾� Ÿ���� ���⿡ �߰�
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

    // ������ �˾����� �����ϴ� ��ųʸ�
    private Dictionary<PopupType, GameObject> activePopups = new Dictionary<PopupType, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �Ѿ�� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �˾��� �����ϰ� ȭ�鿡 ���.
    /// �̹� �� �ִ� ��� �ٽ� ����� ����.
    /// </summary>
    public void ShowPopup(PopupType type)
    {
        if (activePopups.ContainsKey(type))
        {
            activePopups[type].SetActive(true);
            return;
        }

        var entry = popupPrefabs.Find(p => p.type == type);
        if (entry != null && entry.prefab != null)
        {
            GameObject popupInstance = Instantiate(entry.prefab, transform);
            activePopups[type] = popupInstance;
        }
        else
        {
            Debug.LogWarning($"Popup prefab for type {type} not found.");
        }
    }

    /// <summary>
    /// �˾��� �ݽ��ϴ�.
    /// </summary>
    public void HidePopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup))
        {
            popup.SetActive(false);
        }
    }

    /// <summary>
    /// �˾��� �ı��ϰ� ��ųʸ����� ����.
    /// </summary>
    public void DestroyPopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup))
        {
            Destroy(popup);
            activePopups.Remove(type);
        }
    }
}