using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;            // �ε� ����� �� UI
    [SerializeField] private TMP_Text loadingText;         // �ε� �ۼ�Ʈ �ؽ�Ʈ
    [SerializeField] private Image fadeImage;              // ȭ�� ���̵�� �̹��� (���� ��� ��)
    [SerializeField] private float fadeDuration = 1f;      // ���̵� ��/�ƿ� �ð�
    [SerializeField] private float minLoadTime = 2f;       // �� �ε� �Ϸ� �� �ּ� ��� �ð�

    private AsyncOperation loadingOperation;

    private void Start()
    {
        // ȭ�� ���̵� �� �� �ε� ����
        StartCoroutine(FadeIn());
        StartCoroutine(LoadNextSceneAsync(MySceneManager.Instance.NextScene));
    }

    private IEnumerator LoadNextSceneAsync(string sceneName)
    {
        // �� �񵿱� �ε� ���� (Ȱ��ȭ�� ���߿� ��������)
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        bool isLoadingDone = false;      // 0.9f ���� ���� �÷���
        float postLoadTimer = 0f;        // �ε� �Ϸ� �� ��� �ð� ������

        while (!loadingOperation.isDone)
        {
            // ������� 0.0~0.9 ������ ������Ʈ�� (0.9 �̻��� Ȱ��ȭ ���)
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            UpdateProgress(progress); // �����̴� + �ؽ�Ʈ ����

            // �ε� �Ϸ� ����
            if (loadingOperation.progress >= 0.9f)
            {
                if (!isLoadingDone)
                {
                    // ó�� 0.9f ���� ����
                    isLoadingDone = true;
                }
                else
                {
                    // �ε� �Ϸ� ���� �ð� ����
                    postLoadTimer += Time.deltaTime;

                    // �ּ� ��� �ð� ��� �� ���̵� �ƿ� + �� ��ȯ
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
    /// �����̴� �� �ؽ�Ʈ�� �ε� �ۼ�Ʈ ǥ��
    /// </summary>
    private void UpdateProgress(float progress)
    {
        loadingBar.value = progress;
        loadingText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }

    /// <summary>
    /// ���� �� ȭ�� ��Ӱ� �� ��� (���̵� ��)
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
    /// �ε� �Ϸ� �� ȭ�� ��� �� ��Ӱ� (���̵� �ƿ�), ���� �� Ȱ��ȭ
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

        // ���� ������ ��ȯ
        loadingOperation.allowSceneActivation = true;
    }
}