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
            messageText.text = "아이디를 입력하세요.";
            return;
        }

        // 영어, 숫자, 한글, 밑줄만 허용
        if (!Regex.IsMatch(playerName, @"^[a-zA-Z0-9가-힣_]+$"))
        {
            messageText.text = "특수문자는 사용할 수 없습니다.";
            return;
        }

        if (RankingManager.Instance.IsIdAlreadyUsed(playerName))
        {
            messageText.text = "이미 존재하는 아이디입니다.";
            return;
        }

        // 임시 세션에 저장 (랭킹에 아직 등록 안함)
        PlayerSession.Instance.SetPlayerId(playerName);

        messageText.text = "아이디 등록 완료!";
        submitButton.interactable = false;
        nameInputField.interactable = false;

        StartCoroutine(DelayAndStartGame(2f));
    }

    private IEnumerator DelayAndStartGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MySceneManager.Instance.LoadScene("Battle"); // 씬 이름은 실제 사용 중인 것으로 교체
    }
}