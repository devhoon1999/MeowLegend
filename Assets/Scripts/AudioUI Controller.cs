using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeUI : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Button bgmMuteButton;
    [SerializeField] private Image bgmMuteIcon;
    [SerializeField] private Sprite bgmMuteOnSprite;
    [SerializeField] private Sprite bgmMuteOffSprite;

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button sfxMuteButton;
    [SerializeField] private Image sfxMuteIcon;
    [SerializeField] private Sprite sfxMuteOnSprite;
    [SerializeField] private Sprite sfxMuteOffSprite;

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
        if (value <= 0f)
        {
            if (!isBgmMuted)
            {
                isBgmMuted = true;
                UpdateBGMMuteIcon();
            }
        }
        else
        {
            if (isBgmMuted)
            {
                isBgmMuted = false;
                UpdateBGMMuteIcon();
            }
        }
        AudioManager.Instance.SetMuteBGM(isBgmMuted);
        AudioManager.Instance.SetBGMVolume(value);

        if (!isBgmMuted)
            lastBgmVolume = value;
    }

    private void OnSfxSliderChanged(float value)
    {
        if (value <= 0f)
        {
            if (!isSfxMuted)
            {
                isSfxMuted = true;
                UpdateSFXMuteIcon();
            }
        }
        else
        {
            if (isSfxMuted)
            {
                isSfxMuted = false;
                UpdateSFXMuteIcon();
            }
        }
        AudioManager.Instance.SetMuteSFX(isSfxMuted);
        AudioManager.Instance.SetSFXVolume(value);

        if (!isSfxMuted)
            lastSfxVolume = value;
    }

    private void ToggleBGMMute()
    {
        isBgmMuted = !isBgmMuted;
        AudioManager.Instance.SetMuteBGM(isBgmMuted);

        if (isBgmMuted)
        {
            bgmSlider.value = 0f;
        }
        else
        {
            bgmSlider.value = lastBgmVolume > 0f ? lastBgmVolume : 1f;
        }
        UpdateBGMMuteIcon();
    }

    private void ToggleSFXMute()
    {
        isSfxMuted = !isSfxMuted;
        AudioManager.Instance.SetMuteSFX(isSfxMuted);

        if (isSfxMuted)
        {
            sfxSlider.value = 0f;
        }
        else
        {
            sfxSlider.value = lastSfxVolume > 0f ? lastSfxVolume : 1f;
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