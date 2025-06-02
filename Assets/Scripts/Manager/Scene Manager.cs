using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    private string nextScene;
    private readonly string[] scenesWithoutUI = { "Title", "LoadingScene" };
    private const string uiSceneName = "UI";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 외부에서 씬 전환 요청 시 사용
    /// </summary>
    public void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }

    /// <summary>
    /// LoadingScene에서 호출됨 — 다음 씬을 비동기로 로드
    /// </summary>
    public void LoadNextSceneAsync()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation mainSceneOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextScene);
        mainSceneOp.allowSceneActivation = false;

        while (mainSceneOp.progress < 0.9f)
        {
            yield return null;
        }

        // UI 처리
        if (IsSceneWithoutUI(nextScene))
        {
            // 필요 없는 경우 → UI 언로드
            if (IsSceneLoaded(uiSceneName))
            {
                AsyncOperation unloadOp = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(uiSceneName);
                while (!unloadOp.isDone)
                    yield return null;
            }
        }
        else
        {
            // 필요한 경우 → UI 로드
            if (!IsSceneLoaded(uiSceneName))
            {
                AsyncOperation uiLoadOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);
                while (!uiLoadOp.isDone)
                    yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        mainSceneOp.allowSceneActivation = true;
    }

    private bool IsSceneWithoutUI(string sceneName)
    {
        foreach (var s in scenesWithoutUI)
        {
            if (s == sceneName)
                return true;
        }
        return false;
    }

    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}