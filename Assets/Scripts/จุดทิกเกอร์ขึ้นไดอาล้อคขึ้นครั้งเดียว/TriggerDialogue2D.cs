using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TriggerDialogue2D : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;
    public Sprite[] images;

    [Header("Typewriter")]
    public float typingSpeed = 0.04f;

    private int index;
    private bool isTalking;
    private bool isTyping;
    private bool alreadyTriggered;

    private Coroutine typingCoroutine;
    private PlayerController2D player;

    void Start()
    {
        dialoguePanel.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player")
                .GetComponent<PlayerController2D>();
    }

    void Update()
    {
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // กดระหว่างพิมพ์ → ขึ้นข้อความทันที
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (alreadyTriggered) return;

        StartDialogue();
    }

    void StartDialogue()
    {
        alreadyTriggered = true;
        isTalking = true;
        index = 0;

        dialoguePanel.SetActive(true);
        player.canMove = false; // 🔒 ล็อกการเดิน

        dialogueImage.sprite = images[index];
        StartTyping();
    }

    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeLine(lines[index]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        index++;

        if (index < lines.Length)
        {
            dialogueImage.sprite = images[index];
            StartTyping();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;

        player.canMove = true; // 🔓 ปลดล็อกการเดิน
    }
}
