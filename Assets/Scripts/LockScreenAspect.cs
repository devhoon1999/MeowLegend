using UnityEngine;

public class LockScreenAspect : MonoBehaviour
{
    private static LockScreenAspect instance;
    private float targetAspect = 9f / 16f; // 세로 화면 비율
    private int lastWidth;
    private int lastHeight;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }

    private void Update()
    {
        // 창 크기(해상도)가 바뀌었는지 체크
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            AdjustResolution();
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }
    }

    private void AdjustResolution()
    {
        int currentWidth = Screen.width;
        int expectedHeight = Mathf.RoundToInt(currentWidth / targetAspect);

        // 비율 유지가 안되면 해상도 변경
        if (Screen.height != expectedHeight)
        {
            Screen.SetResolution(currentWidth, expectedHeight, false);
        }
    }
}