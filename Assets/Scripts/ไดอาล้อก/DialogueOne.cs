using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueOne : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public GameObject tutorialUI;

    [Header("Dialogue Content")]
    [TextArea(3, 6)]
    public string[] sentences;
    public Sprite[] sentenceImages;

    [Header("Typewriter")]
    public float typingSpeed = 0.05f;

    private int index = 0;
    private bool isTyping = false;
    private bool dialogueEnded = false;
    private bool tutorialShowing = false;
    private bool finished = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialoguePanel.SetActive(true);
        tutorialUI?.SetActive(false);
        ShowSentence();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Return) || finished)
            return;

        // ⌨️ ข้ามพิมพ์
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = sentences[index];
            isTyping = false;
            return;
        }

        // 🗨️ ยังอยู่ในบทสนทนา
        if (!dialogueEnded)
        {
            if (index < sentences.Length - 1)
            {
                index++;
                ShowSentence();
            }
            else
            {
                EndDialogueAndShowTutorial();
            }
            return;
        }

        // ❌ ปิด Tutorial (Enter ครั้งเดียว)
        if (tutorialShowing)
        {
            tutorialUI.SetActive(false);
            tutorialShowing = false;
            finished = true; // 🔒 ล็อกไม่ให้ Enter ทำอะไรอีก
        }
    }

    void EndDialogueAndShowTutorial()
    {
        dialoguePanel.SetActive(false);
        dialogueEnded = true;

        // 📘 เปิด Tutorial อัตโนมัติ
        if (tutorialUI != null)
        {
            tutorialUI.SetActive(true);
            tutorialShowing = true;
        }
    }

    void ShowSentence()
    {
        if (characterImage != null &&
            sentenceImages != null &&
            index < sentenceImages.Length)
        {
            characterImage.sprite = sentenceImages[index];
            characterImage.enabled = true;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentences[index]));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
