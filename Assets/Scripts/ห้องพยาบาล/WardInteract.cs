using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WardInteract : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject interactPanel;
    public Button[] optionButtons;
    public GameObject infoPanel;
    public TMP_Text infoText;

    [Header("Character Dialog")]
    public GameObject dialogPanel;
    public TMP_Text dialogTextUI;
    public string[] characterDialogs;
    public float typingSpeed = 0.05f;

    [Header("UI Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private int round = 0;
    private bool isInteracting = false;
    private int selectedOption = 0;
    private string[][] options;

    private bool isPlayerNearby = false;
    private int dialogIndex = 0;
    private bool isTyping = false;

    // ✅ เพิ่มสถานะใหม่
    private bool isShowingInfo = false;

    void Start()
    {
        interactPanel.SetActive(false);
        infoPanel.SetActive(false);
        dialogPanel.SetActive(false);

        options = new string[3][];
        options[0] = new string[] { "65", "66", "67", "68" };
        options[1] = new string[] { "01", "02", "03", "04" };
        options[2] = new string[] { "Rin", "xxxx", "xxxx", "xxxx" };
    }

    void Update()
    {
        if (isInteracting)
        {
            // --- กำลังเลือกข้อมูล ---
            if (!isShowingInfo && !dialogPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    selectedOption--;
                    if (selectedOption < 0) selectedOption = optionButtons.Length - 1;
                    UpdateButtonColors();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    selectedOption++;
                    if (selectedOption >= optionButtons.Length) selectedOption = 0;
                    UpdateButtonColors();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    round++;
                    if (round < options.Length)
                    {
                        UpdateButtonTexts();
                        selectedOption = 0;
                        UpdateButtonColors();
                    }
                    else
                    {
                        // ✅ จบรอบเลือก → แสดงข้อมูล
                        ShowFinalInfo();
                        isShowingInfo = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CloseInteraction();
                }
            }
            // --- อยู่ในหน้า Info ---
            else if (isShowingInfo && Input.GetKeyDown(KeyCode.Return))
            {
                // ปิด Info Panel → เปิด Dialog
                infoPanel.SetActive(false);
                isShowingInfo = false;
                dialogPanel.SetActive(true);
                dialogIndex = 0;
                StartCoroutine(TypeDialog(characterDialogs[dialogIndex]));
            }
            // --- อยู่ในหน้า Dialog ---
            else if (dialogPanel.activeSelf && Input.GetKeyDown(KeyCode.Return) && !isTyping)
            {
                dialogIndex++;
                if (dialogIndex < characterDialogs.Length)
                {
                    StartCoroutine(TypeDialog(characterDialogs[dialogIndex]));
                }
                else
                {
                    dialogPanel.SetActive(false);
                    CloseInteraction();
                }
            }
        }
        else
        {
            // ✅ เริ่มต้น interaction
            if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
            {
                StartInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        round = 0;
        selectedOption = 0;
        interactPanel.SetActive(true);
        infoPanel.SetActive(false);
        dialogPanel.SetActive(false);
        UpdateButtonTexts();
        UpdateButtonColors();
        Time.timeScale = 0f;
    }

    void UpdateButtonTexts()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[round][i];
        }
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponent<Image>().color = (i == selectedOption) ? selectedColor : normalColor;
        }
    }

    void ShowFinalInfo()
    {
        interactPanel.SetActive(false);
        infoPanel.SetActive(true);
        infoText.text = "Name: Rinlada\n" +
                        "Class : 01\n" +
                        "ID : 6601770\n" +
                        "Behavior : High blood pressure\n" +
                        "Symptoms: Lack of sleep, fatigue";
    }

    IEnumerator TypeDialog(string sentence)
    {
        isTyping = true;
        dialogTextUI.text = "";
        foreach (char c in sentence)
        {
            dialogTextUI.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        isTyping = false;
    }

    void CloseInteraction()
    {
        isInteracting = false;
        interactPanel.SetActive(false);
        infoPanel.SetActive(false);
        dialogPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
