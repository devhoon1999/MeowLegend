using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBt : MonoBehaviour
{
    public void SetingsButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.AudioSettings);
    }
}
