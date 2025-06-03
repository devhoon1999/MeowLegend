using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBT : MonoBehaviour
{
    public void HomeButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.Giveup);
    }
}
