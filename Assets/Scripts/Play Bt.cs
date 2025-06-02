using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBt : MonoBehaviour
{
    public void PlayButton()
    {
        MySceneManager.Instance.LoadScene("Home");
    }
}
