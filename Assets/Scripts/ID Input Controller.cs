using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;

public class NameInputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text messageText;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            messageText.text = "���̵� �Է��ϼ���.";
            return;
        }

        // Ư������ üũ (����, ����, ����(_)�� ���)
        if (!Regex.IsMatch(playerName, @"^[a-zA-Z0-9_]+$"))
        {
            messageText.text = "Ư�����ڴ� ����� �� �����ϴ�.";
            return;
        }

        if (RankingManager.Instance.IsIdAlreadyUsed(playerName))
        {
            messageText.text = "�̹� �����ϴ� ���̵��Դϴ�.";
            return;
        }

        // ��� ���� �� ��ŷ�� �߰�
        RankingManager.Instance.AddPlayerResult(playerName, 0, 1); // �ʱ� ������ ��������
        messageText.text = "���̵� ��� �Ϸ�!";

        StartCoroutine(DelayandPlay(2f));

        IEnumerator DelayandPlay(float sec)
        {
            yield return new WaitForSeconds(sec);
            MySceneManager.Instance.LoadScene("Battle");
        }
    }
}