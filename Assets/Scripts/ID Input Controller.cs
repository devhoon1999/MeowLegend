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
            messageText.text = "아이디를 입력하세요.";
            return;
        }

        // 특수문자 체크 (영어, 숫자, 밑줄(_)만 허용)
        if (!Regex.IsMatch(playerName, @"^[a-zA-Z0-9_]+$"))
        {
            messageText.text = "특수문자는 사용할 수 없습니다.";
            return;
        }

        if (RankingManager.Instance.IsIdAlreadyUsed(playerName))
        {
            messageText.text = "이미 존재하는 아이디입니다.";
            return;
        }

        // 등록 성공 → 랭킹에 추가
        RankingManager.Instance.AddPlayerResult(playerName, 0, 1); // 초기 점수와 스테이지
        messageText.text = "아이디 등록 완료!";

        StartCoroutine(DelayandPlay(2f));

        IEnumerator DelayandPlay(float sec)
        {
            yield return new WaitForSeconds(sec);
            MySceneManager.Instance.LoadScene("Battle");
        }
    }
}