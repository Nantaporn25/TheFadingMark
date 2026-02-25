using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMannequin : MonoBehaviour
{
    [Header("หุ่นที่ต้องตรวจทั้งหมด")]
    public Mannequin[] mannequins;

    [Header("UI Dialog")]
    public GameObject dialogBox;
    public Image profileImageUI;
    public TextMeshProUGUI nameTextUI;
    public TextMeshProUGUI dialogTextUI;

    [Header("โปรไฟล์ของผู้ตรวจ")]
    public Sprite profileImage;
    public string npcName = "หัวหน้าหุ่น";

    [Header("ประโยคเมื่อถูกต้องทั้งหมด (เรียงตามลำดับ)")]
    [TextArea(2, 3)]
    public string[] correctDialogLines;

    //[Header("วัตถุรางวัล (ปิดไว้ก่อนใน Inspector)")]
    //public GameObject rewardObject;

    [Header("วัตถุรางวัลทั้งหมด (ปิดไว้ก่อนใน Inspector)")]
    public GameObject[] rewardObjects;

    private bool playerInRange = false;
    private bool dialogActive = false;
    private int dialogIndex = 0;

    private bool alreadyCompleted = false;

    void Update()
    {
        // 🔥 กด K เพื่อผ่านทันที (Debug)
        if (!alreadyCompleted && Input.GetKeyDown(KeyCode.K))
        {
            alreadyCompleted = true;
            ForceComplete();
            return;
        }

        // ผู้เล่นกด E เพื่อตรวจ
        if (!alreadyCompleted && playerInRange && Input.GetKeyDown(KeyCode.E) && !dialogActive)
        {
            CheckAllMannequins();
        }

        // ❗ อันนี้ต้องทำงานเสมอ แม้เควสเสร็จแล้ว
        if (dialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            NextDialog();
        }
    }

    // ---------------------- ตรวจหุ่นทั้งหมด ----------------------
    public void CheckAllMannequins()
    {
        foreach (var m in mannequins)
        {
            if (!m.IsCorrect)
            {
                ShowDialog("ยังมีหุ่นที่ใส่สีผิดอยู่ ลองตรวจดูอีกครั้งสิ");
                return;
            }
        }

        // ผ่านหมด!
        dialogIndex = 0;
        dialogActive = true;

        if (correctDialogLines.Length > 0)
            ShowDialog(correctDialogLines[dialogIndex]);
    }

    // ---------------------- ไปประโยคถัดไป ----------------------
    void NextDialog()
    {
        dialogIndex++;

        // ถ้ายังไม่ถึงประโยคสุดท้าย → แสดงประโยคต่อไป
        if (dialogIndex < correctDialogLines.Length)
        {
            ShowDialog(correctDialogLines[dialogIndex]);
        }
        // ถ้าครบแล้ว → ปิด Dialog + โชว์รางวัล
        else
        {
            HideDialog();
            GiveReward();
        }
    }

    // ---------------------- แสดงรางวัล ----------------------
    //void GiveReward()
    //{
    //    if (rewardObject != null)
    //    {
    //        rewardObject.SetActive(true);   // ← ปรากฎรางวัลหลังประโยคสุดท้าย
    //        Debug.Log("🎉 รางวัลปรากฎแล้ว!");
    //    }
    //}

    void GiveReward()
    {
        if (rewardObjects == null || rewardObjects.Length == 0) return;

        foreach (GameObject reward in rewardObjects)
        {
            if (reward != null)
            {
                reward.SetActive(true);
                Debug.Log("🎁 เปิดรางวัล: " + reward.name);
            }
        }
    }

    // ---------------------- ระบบไดอะล็อก ----------------------
    void ShowDialog(string msg)
    {
        //if (dialogBox == null) return;

        dialogBox.SetActive(true);
        dialogActive = true;

        if (profileImageUI != null)
            profileImageUI.sprite = profileImage;

        if (nameTextUI != null)
            nameTextUI.text = npcName;

        if (dialogTextUI != null)
            dialogTextUI.text = msg;
    }

    void HideDialog()
    {
        //if (dialogBox == null) return;

        dialogBox.SetActive(false);
        dialogActive = false;
    }

    // ---------------------- ตรวจผู้เล่นเข้าระยะ ----------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideDialog();
        }
    }

    void ForceComplete()
    {
        alreadyCompleted = true;

        if (correctDialogLines.Length > 0)
            ShowDialog(correctDialogLines[0]);

        GiveReward();
    }

}
