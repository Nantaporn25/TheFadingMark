using UnityEngine;
using TMPro;
using System.Collections;
public class DialoguePhone : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    public string[] sentences;
    private int index = 0;

    public string nextSceneName;
    public SceneFader sceneFader; // อ้างอิง SceneFader

    public float typingSpeed = 0.05f;
    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeSentence(sentences[index]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.X))
        {
            if (isTyping)
            {
                // ถ้ายังกำลังพิมพ์อยู่ → ข้ามไปโชว์ทั้งประโยคเลย
                StopAllCoroutines();
                dialogueText.text = sentences[index];
                isTyping = false;
            }
            else
            {
                // ถ้าเขียนครบแล้ว → ไปประโยคถัดไป
                NextSentence();
            }
        }
    }
    void ShowSentence()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences[index]));
    }
    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeSentence(sentences[index]));
        }
        else
        {
            dialoguePanel.SetActive(false);

            // เรียก Fade Out → เปลี่ยนฉาก
            sceneFader.FadeToScene(nextSceneName);
        }
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
