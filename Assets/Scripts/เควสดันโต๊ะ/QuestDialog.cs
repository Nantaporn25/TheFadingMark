using UnityEngine;
using TMPro;

public class QuestDialog : MonoBehaviour
{
    public static QuestDialog instance;

    [Header("UI References")]
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    [Header("Dialog Lines (ตั้งใน Inspector)")]
    [TextArea(2, 5)]
    public string[] lines;

    private int currentLine = 0;
    private bool dialogActive = false;
    private System.Action onDialogEnd;   // callback ตอนจบไดอาล็อก

    void Awake()
    {
        instance = this;
        dialogBox.SetActive(false);  // เริ่มต้นปิดอยู่
    }

    void Update()
    {
        if (!dialogActive) return;

        if (Input.GetKeyDown(KeyCode.Return)) // กด Enter
        {
            currentLine++;

            // ❌ หมดประโยคแล้ว → ปิด dialog
            if (currentLine >= lines.Length)
            {
                dialogActive = false;
                dialogBox.SetActive(false);

                onDialogEnd?.Invoke();
                return;
            }

            // ✔ แสดงประโยคถัดไป
            dialogText.text = lines[currentLine];
        }
    }

    // ✔ เรียกเพื่อเริ่มไดอาล็อก
    public void StartDialog(System.Action onEnd = null)
    {
        onDialogEnd = onEnd;
        currentLine = 0;

        dialogActive = true;
        dialogBox.SetActive(true);

        // แสดงประโยคแรก
        dialogText.text = lines[0];
    }
}
