using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLogView : MonoBehaviour
{
    [SerializeField] private GameObject logPanel;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private Button backButton;

    private readonly StringBuilder logBuilder = new();

    public bool IsOpen => logPanel.activeSelf;

    private void Awake()
    {
        logPanel.SetActive(false);
        backButton.onClick.AddListener(Close);
    }

    public void AddDialogue(string speaker, string text)
    {
        if (logBuilder.Length > 0)
        {
            logBuilder.AppendLine();
        }

        logBuilder.AppendLine(speaker);
        logBuilder.AppendLine(text);

        UpdateText();
    }

    public void AddDescription(string text)
    {
        if (logBuilder.Length > 0)
        {
            logBuilder.AppendLine();
        }

        logBuilder.AppendLine(text);

        UpdateText();
    }

    public void Toggle()
    {
        logPanel.SetActive(!logPanel.activeSelf);
    }

    public void Close()
    {
        logPanel.SetActive(false);
    }

    private void UpdateText()
    {
        logText.text = logBuilder.ToString();
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveListener(Close);
    }
}
