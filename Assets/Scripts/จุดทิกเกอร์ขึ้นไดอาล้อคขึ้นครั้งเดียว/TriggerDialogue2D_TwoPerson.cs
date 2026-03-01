using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TriggerDialogue2D_TwoPerson : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Data")]
    public DialogueLine[] dialogueLines;

    [Header("Typewriter")]
    public float typingSpeed = 0.04f;

    private int index = 0;
    private bool isTalking = false;
    private bool isTyping = false;

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
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueLines[index].sentence;
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

        StartDialogue();
    }

    void StartDialogue()
    {
        isTalking = true;
        index = 0;

        dialoguePanel.SetActive(true);
        player.canMove = false;

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = dialogueLines[index];

        characterImage.sprite = line.characterImage;
        speakerNameText.text = line.characterName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line.sentence));
    }

    IEnumerator TypeLine(string sentence)
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

    void NextLine()
    {
        index++;

        if (index < dialogueLines.Length)
        {
            ShowLine();
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
        player.canMove = true;
        Destroy(gameObject); // ลบ Trigger ทิ้งหลังคุยจบ
    }
}
