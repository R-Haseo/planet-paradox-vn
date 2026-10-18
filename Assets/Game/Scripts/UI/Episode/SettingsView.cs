using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider voiceSlider;

    public bool IsOpen => settingsPanel.activeSelf;

    private void Awake()
    {
        backButton.onClick.AddListener(Close);
        settingsPanel.SetActive(false);
    }

    public void Initialize(
        Action<float> onBgmVolumeChanged,
        Action<float> onSeVolumeChanged,
        Action<float> onVoiceVolumeChanged)
    {
        bgmSlider.onValueChanged.AddListener(value => onBgmVolumeChanged?.Invoke(value));
        seSlider.onValueChanged.AddListener(value => onSeVolumeChanged?.Invoke(value));
        voiceSlider.onValueChanged.AddListener(value => onVoiceVolumeChanged?.Invoke(value));
    }

    public void Toggle()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void Close()
    {
        settingsPanel.SetActive(false);
    }
}
