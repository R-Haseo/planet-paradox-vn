using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioControlView : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button logButton;
    [SerializeField] private Button autoButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button titleButton;
    [SerializeField] private UIButtonSePlayer buttonSePlayer;

    public void Initialize(
        Action onSaveClicked,
        Action onLoadClicked,
        Action onLogClicked,
        Action onAutoClicked,
        Action onSkipClicked,
        Action onSettingsClicked,
        Action onTitleClicked)
    {
        saveButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onSaveClicked?.Invoke();
        });

        loadButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onLoadClicked?.Invoke();
        });

        logButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onLogClicked?.Invoke();
        });

        autoButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onAutoClicked?.Invoke();
        });

        skipButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onSkipClicked?.Invoke();
        });

        settingsButton.onClick.AddListener(() =>
        {
            buttonSePlayer.Play();
            onSettingsClicked?.Invoke();
        });

        titleButton.onClick.AddListener(() =>
        {
            StartCoroutine(ReturnToTitleAfterSe(onTitleClicked));
        });
    }

    private IEnumerator ReturnToTitleAfterSe(Action onTitleClicked)
    {
        yield return buttonSePlayer.PlayAndWait();
        onTitleClicked?.Invoke();
    }
}
