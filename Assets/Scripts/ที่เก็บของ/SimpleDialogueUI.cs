using UnityEngine;
using TMPro;

public class SimpleDialogueUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 4)]
    public string[] lines;

    private int index = 0;
    private bool isOpen = false;

    void Awake()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextLine();
        }
    }

    public void StartDialogue()
    {
        if (lines.Length == 0) return;

        index = 0;
        isOpen = true;
        panel.SetActive(true);

        dialogueText.text = lines[index];
        Time.timeScale = 0f;
    }

    void NextLine()
    {
        index++;

        if (index >= lines.Length)
        {
            EndDialogue();
        }
        else
        {
            dialogueText.text = lines[index];
        }
    }

    void EndDialogue()
    {
        panel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;
    }
}
