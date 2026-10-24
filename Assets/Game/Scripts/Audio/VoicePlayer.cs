using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class VoicePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private string episodeId;

    private AsyncOperationHandle<AudioClip>? currentHandle;
    private int requestVersion;
    private bool isPlaying;
    public bool IsPlaying => isPlaying;
    private float defaultVolume;
    private float currentVolumeScale = 1.0f;
    private float userVolume = 1.0f;

    private void Awake()
    {
        defaultVolume = audioSource.volume;
    }

    public event Action PlaybackCompleted;

    public void Initialize(string episodeId)
    {
        this.episodeId = episodeId;
    }

    public void Play(string id, float? volume = null)
    {
        currentVolumeScale = volume ?? 1.0f;

        Stop();

        var address = $"voice/{episodeId}/{id}";
        var version = ++requestVersion;

        var handle = Addressables.LoadAssetAsync<AudioClip>(address);

        currentHandle = handle;

        handle.Completed += completedHandle =>
        {
            if (version != requestVersion)
            {
                return;
            }

            if (completedHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"Failed to load voice: {address}");
                ReleaseCurrentHandle();
                return;
            }

            audioSource.clip = completedHandle.Result;
            ApplyVolume();
            audioSource.Play();
            isPlaying = true;
        };
    }

    private void Update()
    {
        if (!isPlaying || audioSource.isPlaying)
        {
            return;
        }

        isPlaying = false;
        PlaybackCompleted?.Invoke();
    }

    public void Stop()
    {
        requestVersion++;
        isPlaying = false;

        audioSource.Stop();
        audioSource.clip = null;

        ReleaseCurrentHandle();
    }

    private void ReleaseCurrentHandle()
    {
        if (!currentHandle.HasValue)
        {
            return;
        }

        var handle = currentHandle.Value;

        if (handle.IsValid())
        {
            Addressables.Release(handle);
        }

        currentHandle = null;
    }

    public void SetUserVolume(float volume)
    {
        userVolume = Mathf.Clamp(volume, 0f, 2f);
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        audioSource.volume = Mathf.Clamp01(defaultVolume * currentVolumeScale * userVolume);
    }

    private void OnDestroy()
    {
        Stop();
    }
}
