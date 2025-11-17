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

    private bool playerInRange = false;
    private bool dialogActive = false;   // <-- เพิ่มตรงนี้

    void Update()
    {
        // กด E เพื่อตรวจ เมื่อผู้เล่นอยู่ในระยะ
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !dialogActive)
        {
            CheckAllMannequins();
        }

        // กด Enter เพื่อปิดไดอะล็อก
        if (dialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            HideDialog();
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

        ShowDialog("เยี่ยมมาก! ทุกหุ่นใส่สีถูกต้องแล้ว!");
        GiveReward();
    }

    void GiveReward()
    {
        // ใส่รางวัล เช่น เปิดประตู
    }

    // ---------------------- ระบบไดอะล็อก ----------------------
    void ShowDialog(string msg)
    {
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
            HideDialog(); // ออกระยะก็ปิดด้วย
        }
    }
}
