using UnityEngine;

public class PopupCanvasReporter : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (PopupManager.Instance != null && canvas != null)
        {
            PopupManager.Instance.SetCanvas(canvas);
        }
    }
}