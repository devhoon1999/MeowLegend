using UnityEngine;

public class QuitGameBt : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        // 에디터에서 실행 중일 경우
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 실행파일에서
        Application.Quit();
#endif
    }
}