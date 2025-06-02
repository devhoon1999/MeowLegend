using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public enum BGMType
{
    MainTheme,
    Battle,
    Boss
}

public enum SFXType
{
    Click,
    Explosion,
    Jump,
    Hit
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Initial Volumes")]
    [Range(0f, 1f)] public float bgmVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private bool isMutedAll = false;
    private bool isMutedBGM = false;
    private bool isMutedSFX = false;

    private Dictionary<BGMType, AudioClip> bgmClips = new();
    private Dictionary<SFXType, AudioClip> sfxClips = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllAudio()
    {
        foreach (BGMType type in System.Enum.GetValues(typeof(BGMType)))
        {
            Addressables.LoadAssetAsync<AudioClip>($"Audio/BGM/{type}").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    bgmClips[type] = handle.Result;
            };
        }

        foreach (SFXType type in System.Enum.GetValues(typeof(SFXType)))
        {
            Addressables.LoadAssetAsync<AudioClip>($"Audio/SFX/{type}").Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    sfxClips[type] = handle.Result;
            };
        }
    }

    public void PlayBGM(BGMType type, bool loop = true)
    {
        if (bgmClips.TryGetValue(type, out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.volume = isMutedAll || isMutedBGM ? 0f : bgmVolume;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxClips.TryGetValue(type, out var clip))
        {
            sfxSource.PlayOneShot(clip, isMutedAll || isMutedSFX ? 0f : sfxVolume);
        }
    }

    public void SetMasterMute(bool mute)
    {
        isMutedAll = mute;
        ApplyVolumes();
    }

    public void SetBGMMute(bool mute)
    {
        isMutedBGM = mute;
        ApplyVolumes();
    }

    public void SetSFXMute(bool mute)
    {
        isMutedSFX = mute;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    private void ApplyVolumes()
    {
        bgmSource.volume = isMutedAll || isMutedBGM ? 0f : bgmVolume;
        // sfxSource는 PlayOneShot이라 실시간 볼륨 반영 불가
    }
}
