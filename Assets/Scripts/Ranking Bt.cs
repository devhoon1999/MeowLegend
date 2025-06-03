using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBt : MonoBehaviour
{
    public void RankingButton()
    {
        PopupManager.Instance.ShowPopup(PopupType.Ranking);
    }
}
