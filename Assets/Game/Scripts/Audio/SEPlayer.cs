using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SEPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private float userVolume = 1.0f;

    public void Play(string assetId, float? volume = null)
    {
        StartCoroutine(PlayCoroutine(assetId, volume));
    }

    private IEnumerator PlayCoroutine(string assetId, float? volume)
    {
        var address = $"se/{assetId}";
        var handle = Addressables.LoadAssetAsync<AudioClip>(address);

        yield return handle;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load SE: {address}");
            Addressables.Release(handle);
            yield break;
        }

        var clip = handle.Result;

        audioSource.PlayOneShot(clip, Mathf.Clamp((volume ?? 1.0f) * userVolume, 0f, 2f));

        yield return new WaitForSecondsRealtime(clip.length);

        Addressables.Release(handle);
    }

    public void SetUserVolume(float volume)
    {
        userVolume = Mathf.Clamp01(volume);
    }
}
