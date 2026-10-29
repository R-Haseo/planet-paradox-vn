using TMPro;
using UnityEngine;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private GameObject speakerPanel;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    public void Show(string speaker, string text)
    {
        speakerPanel.SetActive(!string.IsNullOrWhiteSpace(speaker));
        speakerText.text = speaker;
        dialogueText.text = text;
    }
}
