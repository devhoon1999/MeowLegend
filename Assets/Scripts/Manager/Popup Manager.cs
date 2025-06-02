using UnityEngine;
using System.Collections.Generic;

public enum PopupType
{
    AudioSettings
    // 필요한 팝업 타입을 여기에 추가
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

    // 생성된 팝업들을 저장하는 딕셔너리
    private Dictionary<PopupType, GameObject> activePopups = new Dictionary<PopupType, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬을 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 팝업을 생성하고 화면에 띄움.
    /// 이미 떠 있는 경우 다시 띄우지 않음.
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
    /// 팝업을 닫습니다.
    /// </summary>
    public void HidePopup(PopupType type)
    {
        if (activePopups.TryGetValue(type, out GameObject popup))
        {
            popup.SetActive(false);
        }
    }

    /// <summary>
    /// 팝업을 파괴하고 딕셔너리에서 제거.
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