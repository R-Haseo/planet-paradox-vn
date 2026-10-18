using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private float defaultVolume;
    private float userVolume = 1.0f;
    private float currentVolumeScale = 1.0f;
    private string currentAssetId;
    private AsyncOperationHandle<AudioClip>? currentHandle;
    private int requestVersion;

    private void Awake()
    {
        defaultVolume = audioSource.volume;
    }

    public void Play(string assetId, float? volume = null)
    {
        currentVolumeScale = volume ?? 1.0f;

        if (currentAssetId == assetId && audioSource.isPlaying)
        {
            ApplyVolume();
            return;
        }

        StopCurrent();

        currentAssetId = assetId;

        var address = $"bgm/{assetId}";
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
                Debug.LogError($"Failed to load BGM: {address}");
                ReleaseCurrentHandle();
                return;
            }

            audioSource.clip = completedHandle.Result;
            audioSource.loop = true;
            ApplyVolume();
            audioSource.Play();
        };
    }

    public void Stop()
    {
        requestVersion++;
        currentAssetId = null;
        StopCurrent();
    }

    private void StopCurrent()
    {
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
        requestVersion++;
        StopCurrent();
    }
}
