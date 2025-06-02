using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

public enum AudioClipName
{
    BGM1
    // 필요한 오디오 이름들을 여기에 추가
}

public enum AudioType
{
    BGM,
    SFX
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private Coroutine bgmFadeCoroutine;

    private Dictionary<AudioClipName, string> addressableKeys = new Dictionary<AudioClipName, string>()
    {
        { AudioClipName.BGM1, "Sail_to_the_Stars" },
        // 실제 Addressable key와 enum 매칭
    };

    private bool isBgmMuted = false;
    private bool isSfxMuted = false;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.outputAudioMixerGroup = bgmMixerGroup;
            bgmSource.loop = true;
            bgmSource.volume = 0f;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
            sfxSource.loop = false;

            PlayBGM(AudioClipName.BGM1, 1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClipName clipName, float fadeDuration = 1f)
    {
        if (bgmFadeCoroutine != null)
            StopCoroutine(bgmFadeCoroutine);

        if (!addressableKeys.TryGetValue(clipName, out string key))
        {
            Debug.LogWarning($"BGM key not found for {clipName}");
            return;
        }

        bgmFadeCoroutine = StartCoroutine(SwapBGMCoroutine(key, fadeDuration));
    }

    private IEnumerator SwapBGMCoroutine(string newBGMKey, float fadeDuration)
    {
        yield return StartCoroutine(FadeOutCoroutine(fadeDuration));

        var handle = Addressables.LoadAssetAsync<AudioClip>(newBGMKey);
        yield return handle;

        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            bgmSource.clip = handle.Result;
            bgmSource.Play();

            yield return StartCoroutine(FadeInCoroutine(fadeDuration));
        }
        else
        {
            Debug.LogWarning($"Failed to load BGM: {newBGMKey}");
        }
    }

    private IEnumerator FadeInCoroutine(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, bgmVolume, timer / duration);
            yield return null;
        }
        bgmSource.volume = bgmVolume;
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float timer = 0f;
        float startVolume = bgmSource.volume;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }
        bgmSource.volume = 0f;
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClipName clipName)
    {
        if (isSfxMuted) return;

        if (!addressableKeys.TryGetValue(clipName, out string key))
        {
            Debug.LogWarning($"SFX key not found for {clipName}");
            return;
        }

        StartCoroutine(PlaySFXCoroutine(key));
    }

    private IEnumerator PlaySFXCoroutine(string key)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        yield return handle;

        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            sfxSource.PlayOneShot(handle.Result, sfxVolume);
        }
        else
        {
            Debug.LogWarning($"Failed to load SFX: {key}");
        }
    }

    // 전체 음소거 토글 (BGM, SFX 둘 다)
    public void SetMuteAll(bool mute)
    {
        SetMuteBGM(mute);
        SetMuteSFX(mute);
    }

    public void SetMuteBGM(bool mute)
    {
        isBgmMuted = mute;

        if (mute)
        {
            bgmSource.volume = 0f;
        }
        else
        {
            bgmSource.volume = bgmVolume;
        }
        bgmSource.mute = false; // mute 대신 볼륨 제어로 음소거 처리
    }

    public void SetMuteSFX(bool mute)
    {
        isSfxMuted = mute;
        // SFX 음소거는 isSfxMuted 플래그로 재생 시 판단
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (!isBgmMuted)
            bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public bool IsBgmMuted()
    {
        return isBgmMuted;
    }

    public bool IsSfxMuted()
    {
        return isSfxMuted;
    }
}