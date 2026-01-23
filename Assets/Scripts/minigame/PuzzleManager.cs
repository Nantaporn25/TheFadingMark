using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuzzleManger : MonoBehaviour
{
    [Header("Pictures")]
    public PicDrag picFront;
    public PicDrag picLeft;
    public PicDrag picRight;

    [Header("Dialog UI")]
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    [TextArea(2, 5)]
    public string[] dialogLines;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f;

    [Header("Next Scene")]
    public string nextSceneName;

    private int currentLine = 0;
    private bool dialogActive = false;
    private bool minigameCompleted = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogBox.SetActive(false);
    }

    void Update()
    {
        // ตรวจว่าภาพทั้งหมดล็อกครบ → ถือว่ามินิเกมเสร็จ
        if (!minigameCompleted && picFront.locked && picLeft.locked && picRight.locked)
        {
            minigameCompleted = true;
            StartDialog();
        }

        // กด Enter เพื่อ skip / ไปต่อ
        if (dialogActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                dialogText.text = dialogLines[currentLine];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialog()
    {
        dialogActive = true;
        currentLine = 0;
        dialogBox.SetActive(true);
        StartTyping(dialogLines[currentLine]);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogLines.Length)
        {
            StartTyping(dialogLines[currentLine]);
        }
        else
        {
            EndDialog();
        }
    }

    void EndDialog()
    {
        dialogActive = false;
        dialogBox.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void StartTyping(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";

        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
