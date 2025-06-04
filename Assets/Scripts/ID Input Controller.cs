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
            messageText.text = "¾ÆÀÌµð¸¦ ÀÔ·ÂÇÏ¼¼¿ä.";
            return;
        }

        // ¿µ¾î, ¼ýÀÚ, ÇÑ±Û, ¹ØÁÙ¸¸ Çã¿ë
        if (!Regex.IsMatch(playerName, @"^[a-zA-Z0-9°¡-ÆR_]+$"))
        {
            messageText.text = "Æ¯¼ö¹®ÀÚ´Â »ç¿ëÇÒ ¼ö ¾ø½À´Ï´Ù.";
            return;
        }

        if (RankingManager.Instance.IsIdAlreadyUsed(playerName))
        {
            messageText.text = "ÀÌ¹Ì Á¸ÀçÇÏ´Â ¾ÆÀÌµðÀÔ´Ï´Ù.";
            return;
        }

        // ÀÓ½Ã ¼¼¼Ç¿¡ ÀúÀå (·©Å·¿¡ ¾ÆÁ÷ µî·Ï ¾ÈÇÔ)
        PlayerSession.Instance.SetPlayerId(playerName);

        messageText.text = "¾ÆÀÌµð µî·Ï ¿Ï·á!";
        submitButton.interactable = false;
        nameInputField.interactable = false;

        StartCoroutine(DelayAndStartGame(2f));
    }

    private IEnumerator DelayAndStartGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MySceneManager.Instance.LoadScene("Battle"); // ¾À ÀÌ¸§Àº ½ÇÁ¦ »ç¿ë ÁßÀÎ °ÍÀ¸·Î ±³Ã¼
    }
}