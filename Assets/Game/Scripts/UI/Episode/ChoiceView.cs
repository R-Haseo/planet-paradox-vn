using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceView : MonoBehaviour
{
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private UIButtonSePlayer buttonSePlayer;
    public bool IsOpen => choicePanel.activeSelf;

    public void Show(
        ScenarioChoiceOptionDto[] options,
        Action<ScenarioChoiceOptionDto> onSelected)
    {
        ClearButtons();

        foreach (var option in options)
        {
            var button = Instantiate(choiceButtonPrefab, choiceContainer);

            var text = button.GetComponentInChildren<TMP_Text>();
            text.text = option.Text;

            button.onClick.AddListener(() =>
            {
                buttonSePlayer.Play();

                Hide();
                onSelected?.Invoke(option);
            });
        }

        choicePanel.SetActive(true);
    }

    public void Hide()
    {
        choicePanel.SetActive(false);
        ClearButtons();
    }

    private void ClearButtons()
    {
        for (var i = choiceContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceContainer.GetChild(i).gameObject);
        }
    }
}
