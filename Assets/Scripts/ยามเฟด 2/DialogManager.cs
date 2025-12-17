using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    public GameObject dialogPanel;
    public Image characterImageUI;
    public TextMeshProUGUI characterNameUI;
    public TextMeshProUGUI dialogTextUI;

    [Header("Typewriter")]
    public float typingSpeed = 0.04f;

    private Queue<DialogueLine> dialogueLines;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentSentence;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialogPanel.SetActive(false);
        dialogueLines = new Queue<DialogueLine>();
    }

    private void Update()
    {
        if (!dialogPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // ถ้ากำลังพิมพ์ → แสดงข้อความเต็มทันที
                StopCoroutine(typingCoroutine);
                dialogTextUI.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
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
        characterImageUI.sprite = line.characterImage;

        currentSentence = line.sentence;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogTextUI.text = "";

        foreach (char c in sentence)
        {
            dialogTextUI.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
