using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;            // 로딩 진행률 바 UI
    [SerializeField] private TMP_Text loadingText;         // 로딩 퍼센트 텍스트
    [SerializeField] private Image fadeImage;              // 화면 페이드용 이미지 (검은 배경 등)
    [SerializeField] private float fadeDuration = 1f;      // 페이드 인/아웃 시간
    [SerializeField] private float minLoadTime = 2f;       // 씬 로딩 완료 후 최소 대기 시간

    private AsyncOperation loadingOperation;

    private void Start()
    {
        // 화면 페이드 인 후 로딩 시작
        StartCoroutine(FadeIn());
        StartCoroutine(LoadNextSceneAsync(MySceneManager.Instance.NextScene));
    }

    private IEnumerator LoadNextSceneAsync(string sceneName)
    {
        // 씬 비동기 로드 시작 (활성화는 나중에 수동으로)
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        bool isLoadingDone = false;      // 0.9f 도달 여부 플래그
        float postLoadTimer = 0f;        // 로딩 완료 후 대기 시간 측정용

        while (!loadingOperation.isDone)
        {
            // 진행률은 0.0~0.9 까지만 업데이트됨 (0.9 이상은 활성화 대기)
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            UpdateProgress(progress); // 슬라이더 + 텍스트 갱신

            // 로딩 완료 상태
            if (loadingOperation.progress >= 0.9f)
            {
                if (!isLoadingDone)
                {
                    // 처음 0.9f 도달 시점
                    isLoadingDone = true;
                }
                else
                {
                    // 로딩 완료 이후 시간 누적
                    postLoadTimer += Time.deltaTime;

                    // 최소 대기 시간 경과 시 페이드 아웃 + 씬 전환
                    if (postLoadTimer >= minLoadTime)
                    {
                        yield return StartCoroutine(FadeOutAndActivateScene());
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// 슬라이더 및 텍스트로 로딩 퍼센트 표시
    /// </summary>
    private void UpdateProgress(float progress)
    {
        loadingBar.value = progress;
        loadingText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }

    /// <summary>
    /// 시작 시 화면 어둡게 → 밝게 (페이드 인)
    /// </summary>
    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = 1f - (t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    /// <summary>
    /// 로딩 완료 후 화면 밝게 → 어둡게 (페이드 아웃), 이후 씬 활성화
    /// </summary>
    private IEnumerator FadeOutAndActivateScene()
    {
        float t = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        // 다음 씬으로 전환
        loadingOperation.allowSceneActivation = true;
    }
}