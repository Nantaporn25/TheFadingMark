using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class InteractDialogue : MonoBehaviour
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

    private bool playerInRange;
    private bool isTalking;
    private bool isTyping;
    private int index;

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
        // กด E เพื่อเริ่มคุย
        if (playerInRange && !isTalking && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        // ระหว่างคุย กด Enter
        if (!isTalking) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
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
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void StartDialogue()
    {
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
        player.canMove = true; // 🔓 เดินได้อีก
    }
}
