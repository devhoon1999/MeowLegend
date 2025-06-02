using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeUI : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Button bgmMuteButton;
    [SerializeField] private Image bgmMuteIcon;
    [SerializeField] private Sprite bgmMuteOnSprite;     // BGM 음소거 해제 아이콘
    [SerializeField] private Sprite bgmMuteOffSprite;    // BGM 음소거 아이콘

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button sfxMuteButton;
    [SerializeField] private Image sfxMuteIcon;
    [SerializeField] private Sprite sfxMuteOnSprite;     // SFX 음소거 해제 아이콘
    [SerializeField] private Sprite sfxMuteOffSprite;    // SFX 음소거 아이콘

    private bool isBgmMuted = false;
    private bool isSfxMuted = false;

    private float lastBgmVolume = 1f;
    private float lastSfxVolume = 1f;

    private void Start()
    {
        bgmSlider.value = AudioManager.Instance.GetBGMVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();

        isBgmMuted = AudioManager.Instance.IsBgmMuted();
        isSfxMuted = AudioManager.Instance.IsSfxMuted();

        lastBgmVolume = bgmSlider.value;
        lastSfxVolume = sfxSlider.value;

        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);

        bgmMuteButton.onClick.AddListener(ToggleBGMMute);
        sfxMuteButton.onClick.AddListener(ToggleSFXMute);

        UpdateBGMMuteIcon();
        UpdateSFXMuteIcon();
    }

    private void OnBgmSliderChanged(float value)
    {
        if (isBgmMuted && value > 0f)
        {
            isBgmMuted = false;
            UpdateBGMMuteIcon();
            AudioManager.Instance.SetMuteBGM(false);
        }
        lastBgmVolume = value;
        AudioManager.Instance.SetBGMVolume(value);
    }

    private void OnSfxSliderChanged(float value)
    {
        if (isSfxMuted && value > 0f)
        {
            isSfxMuted = false;
            UpdateSFXMuteIcon();
            AudioManager.Instance.SetMuteSFX(false);
        }
        lastSfxVolume = value;
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void ToggleBGMMute()
    {
        if (isBgmMuted)
        {
            isBgmMuted = false;
            AudioManager.Instance.SetMuteBGM(false);
            AudioManager.Instance.SetBGMVolume(lastBgmVolume);
            bgmSlider.value = lastBgmVolume;
        }
        else
        {
            isBgmMuted = true;
            lastBgmVolume = bgmSlider.value;
            AudioManager.Instance.SetMuteBGM(true);
            AudioManager.Instance.SetBGMVolume(0f);
            bgmSlider.value = 0f;
        }
        UpdateBGMMuteIcon();
    }

    private void ToggleSFXMute()
    {
        if (isSfxMuted)
        {
            isSfxMuted = false;
            AudioManager.Instance.SetMuteSFX(false);
            AudioManager.Instance.SetSFXVolume(lastSfxVolume);
            sfxSlider.value = lastSfxVolume;
        }
        else
        {
            isSfxMuted = true;
            lastSfxVolume = sfxSlider.value;
            AudioManager.Instance.SetMuteSFX(true);
            AudioManager.Instance.SetSFXVolume(0f);
            sfxSlider.value = 0f;
        }
        UpdateSFXMuteIcon();
    }

    private void UpdateBGMMuteIcon()
    {
        bgmMuteIcon.sprite = isBgmMuted ? bgmMuteOffSprite : bgmMuteOnSprite;
    }

    private void UpdateSFXMuteIcon()
    {
        sfxMuteIcon.sprite = isSfxMuted ? sfxMuteOffSprite : sfxMuteOnSprite;
    }
}