using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBt : MonoBehaviour
{
    public void PauseButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.PauseMenu);
    }
}
