using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueOne : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject tutorialUI;

    [Header("Dialogue Settings")]
    [TextArea(3, 6)]
    public string[] sentences;
    public bool isTutorial = true;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f;

    //[Header("Typing Sound")]
    //public AudioSource typingAudioSource;   // เสียงพิมพ์เฉพาะตัวนี้
    //public AudioClip typingSound;           // ไฟล์เสียงพิมพ์ (blip / tick)
    //public float typingSoundDelay = 0.07f;  // ระยะห่างระหว่างเสียงแต่ละตัว

    private int index = 0;
    private bool isTyping = false;
    private bool tutorialActive = false;   // ขณะ Tutorial เปิดอยู่
    private bool tutorialShown = false;    // เปิดแล้วครั้งเดียว
    private Coroutine typingCoroutine;

    void Start()
    {
        // เปิด DialoguePanel ตอนเริ่ม
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        // ปิด TutorialUI ตอนเริ่ม
        if (tutorialUI != null)
            tutorialUI.SetActive(false);

        // ถ้าไม่มีประโยคใน Inspector ให้ใส่ค่าเริ่มต้น
        if (sentences.Length == 0)
        {
            sentences = new string[3];
            sentences[0] = "สวัสดีผู้เล่น";
            sentences[1] = "นี่คือเกมตัวอย่าง";
            sentences[2] = "พร้อมเริ่ม Tutorial แล้ว";
        }

        // เริ่มพิมพ์ประโยคแรก
        StartTyping(sentences[index]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.X))
        {
            if (isTyping)
            {
                // ถ้ากำลังพิมพ์ → แสดงข้อความเต็มทันที
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
               
            }
            else if (!tutorialActive)
            {
                NextSentence();
            }
            else
            {
                // ปิด Tutorial UI
                tutorialUI.SetActive(false);
                tutorialActive = false;
            }
        }
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartTyping(sentences[index]);
        }
        else
        {
            // จบไดอาล็อก
            dialoguePanel.SetActive(false);

            // เปิด Tutorial UI แค่ครั้งเดียว
            if (isTutorial && tutorialUI != null && !tutorialShown)
            {
                tutorialUI.SetActive(true);
                tutorialActive = true;
                tutorialShown = true; // ป้องกันเปิดซ้ำ
            }
        }
    }

    void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
