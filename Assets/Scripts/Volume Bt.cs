using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeBt : MonoBehaviour
{
    public void VolumeButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.AudioSettings);
    }
}
