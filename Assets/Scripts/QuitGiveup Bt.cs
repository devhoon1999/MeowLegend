using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGiveupBt : MonoBehaviour
{
   public void QuitGiveupButton()
    {
        PopupManager.Instance.ClosePopup(PopupType.Giveup);
    }
}
