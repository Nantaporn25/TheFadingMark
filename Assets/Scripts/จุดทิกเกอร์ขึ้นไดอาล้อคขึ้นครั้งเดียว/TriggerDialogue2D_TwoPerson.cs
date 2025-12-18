using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TriggerDialogue2D_TwoPerson : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Image dialogueImage;

    [Header("Character A Expressions")]
    public Sprite[] characterA_Expressions;

    [Header("Character B Expressions")]
    public Sprite[] characterB_Expressions;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    public Speaker[] speakers;
    public Expression[] expressions;

    [Header("Typewriter")]
    public float typingSpeed = 0.04f;

    private int index;
    private bool isTalking;
    private bool isTyping;
    private bool alreadyTriggered;

    private Coroutine typingCoroutine;
    private PlayerController2D player;

    public enum Speaker
    {
        A,
        B
    }

    public enum Expression
    {
        Normal,
        Happy,
        Angry,
        Sad,
        Shock
    }

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
        player.canMove = false;

        UpdateSpeakerAndExpression();
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
            UpdateSpeakerAndExpression();
            StartTyping();
        }
        else
        {
            EndDialogue();
        }
    }

    void UpdateSpeakerAndExpression()
    {
        int expressionIndex = (int)expressions[index];

        if (speakers[index] == Speaker.A)
        {
            dialogueImage.sprite = characterA_Expressions[expressionIndex];
            dialogueText.alignment = TextAlignmentOptions.Left;
        }
        else
        {
            dialogueImage.sprite = characterB_Expressions[expressionIndex];
            dialogueText.alignment = TextAlignmentOptions.Right;
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isTalking = false;
        player.canMove = true;
    }
}
