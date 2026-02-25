using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;

    [Header("Dialogue UI")]
    public TextMeshProUGUI dialogueText;

    [Header("Typing Settings")]
    public float typingSpeed = 0.05f; // ยิ่งน้อยยิ่งเร็ว

    [System.Serializable]
    public class DialogueSet
    {
        [TextArea(2, 5)]
        public string[] sentences;
    }

    [Header("Dialogues per Video")]
    public DialogueSet[] allDialogues;

    private int currentVideoIndex = 0;
    private int currentDialogueIndex = 0;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullSentence;

    void Start()
    {
        PlayVideo(0);
        StartTyping();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // ถ้ากำลังพิมพ์อยู่ → แสดงข้อความเต็มทันที
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentFullSentence;
                isTyping = false;
            }
            else
            {
                NextDialogue();
            }
        }
    }

    void NextDialogue()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex >= allDialogues[currentVideoIndex].sentences.Length)
        {
            currentVideoIndex++;

            if (currentVideoIndex >= videoClips.Length)
            {
                EndGame();
                return;
            }

            PlayVideo(currentVideoIndex);
            currentDialogueIndex = 0;
        }

        StartTyping();
    }

    void StartTyping()
    {
        currentFullSentence =
            allDialogues[currentVideoIndex].sentences[currentDialogueIndex];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in currentFullSentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void PlayVideo(int index)
    {
        videoPlayer.clip = videoClips[index];
        videoPlayer.Play();
    }

    void EndGame()
    {
        SceneManager.LoadScene("MenuScene"); // เปลี่ยนชื่อซีนได้
    }
}