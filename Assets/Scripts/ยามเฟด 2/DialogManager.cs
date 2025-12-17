using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public GameObject dialogPanel;
    public Image characterImageUI;
    public TextMeshProUGUI characterNameUI;
    public TextMeshProUGUI dialogTextUI;

    private Queue<DialogueLine> dialogueLines;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogPanel.SetActive(false);
        dialogueLines = new Queue<DialogueLine>();
    }

    private void Update()
    {
        // กด Enter เพื่อไปประโยคถัดไป
        if (dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextLine();
        }
    }

    public void StartDialog(List<DialogueLine> lines)
    {
        Time.timeScale = 0f;

        dialogPanel.SetActive(true);
        dialogueLines.Clear();

        foreach (DialogueLine line in lines)
            dialogueLines.Enqueue(line);

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialog();
            return;
        }

        DialogueLine line = dialogueLines.Dequeue();
        characterNameUI.text = line.characterName;
        dialogTextUI.text = line.sentence;
        characterImageUI.sprite = line.characterImage;
    }

    private void EndDialog()
    {
        dialogPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
