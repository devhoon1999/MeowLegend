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
        Time.timeScale = 1f;
    }

    private void OnSubmit()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            messageText.text = "���̵� �Է��ϼ���.";
            return;
        }

        // ����, ����, �ѱ�, ���ٸ� ���
        if (!Regex.IsMatch(playerName, @"^[a-zA-Z0-9��-�R_]+$"))
        {
            messageText.text = "Ư�����ڴ� ����� �� �����ϴ�.";
            return;
        }

        if (RankingManager.Instance.IsIdAlreadyUsed(playerName))
        {
            messageText.text = "�̹� �����ϴ� ���̵��Դϴ�.";
            return;
        }

        // �ӽ� ���ǿ� ���� (��ŷ�� ���� ��� ����)
        PlayerSession.Instance.SetPlayerId(playerName);

        messageText.text = "���̵� ��� �Ϸ�!";
        submitButton.interactable = false;
        nameInputField.interactable = false;

        StartCoroutine(DelayAndStartGame(2f));
    }

    private IEnumerator DelayAndStartGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MySceneManager.Instance.LoadScene("Battle"); // �� �̸��� ���� ��� ���� ������ ��ü
    }
}