using UnityEngine;
using TMPro;
using System.Collections;

public class DialoguePhone_Scroll : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] sentences;

    [Header("Typing")]
    public float typingSpeed = 0.05f;

    [Header("Scene")]
    public string nextSceneName;
    public SceneFader sceneFader;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = "";

        StartTypingCurrentLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // ข้าม typewriter → แสดงทั้งบรรทัด
                StopCoroutine(typingCoroutine);
                AppendFullLine(sentences[index]);
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartTypingCurrentLine()
    {
        typingCoroutine = StartCoroutine(TypeLine(sentences[index]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        dialogueText.text += "\n\n"; // เว้นบรรทัด
        isTyping = false;
    }

    void AppendFullLine(string line)
    {
        dialogueText.text += line + "\n\n";
    }

    void NextLine()
    {
        index++;

        if (index < sentences.Length)
        {
            StartTypingCurrentLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (sceneFader != null)
        {
            sceneFader.FadeToScene(nextSceneName);
        }
    }
}
