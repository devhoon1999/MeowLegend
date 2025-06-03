using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBt : MonoBehaviour
{
    public void PlayButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.IDInputfield);
    }
}
