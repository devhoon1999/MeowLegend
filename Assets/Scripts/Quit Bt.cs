using UnityEngine;

public class QuitGameBt : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // �����Ϳ��� ���� ���� ���
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����� �������Ͽ���
        Application.Quit();
#endif
    }
}