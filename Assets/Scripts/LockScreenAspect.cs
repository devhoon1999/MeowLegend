using UnityEngine;

public class LockScreenAspect : MonoBehaviour
{
    private static LockScreenAspect instance;
    private float targetAspect = 9f / 16f; // ���� ȭ�� ����
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
        // â ũ��(�ػ�)�� �ٲ������ üũ
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

        // ���� ������ �ȵǸ� �ػ� ����
        if (Screen.height != expectedHeight)
        {
            Screen.SetResolution(currentWidth, expectedHeight, false);
        }
    }
}