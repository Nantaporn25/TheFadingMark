using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Mannequin : MonoBehaviour
{
    [Header("ข้อมูลตัวละคร")]
    public Sprite profileImage;
    public string characterName;

    [Header("ประโยคไดอาล็อกของตัวละครนี้")]
    [TextArea(2, 5)]
    public string[] dialogLines;

    [Header("ความต้องการของหุ่น (สีที่ถูกต้อง)")]
    public string desiredColor;          // เช่น "Red", "Blue", "Green"
    public Item equippedNecklace;        // สร้อยที่ใส่อยู่ตอนนี้

    [Header("สถานะเควส")]
    public bool IsCorrect = false;       // <-- MainMannequin ต้องใช้ตัวนี้

    [Header("UI อ้างอิง")]
    public GameObject dialogBox;
    public Image profileImageUI;
    public TextMeshProUGUI nameTextUI;
    public TextMeshProUGUI dialogTextUI;

    private int dialogIndex = 0;

    // ------------------------- แสดงไดอาล็อก -------------------------
    public void StartDialog()
    {
        if (dialogLines == null || dialogLines.Length == 0) return;

        dialogBox.SetActive(true);

        if (profileImageUI != null)
            profileImageUI.sprite = profileImage;

        if (nameTextUI != null)
            nameTextUI.text = characterName;

        dialogIndex = 0;
        dialogTextUI.text = dialogLines[dialogIndex];
    }

    public void StopDialog()
    {
        dialogBox.SetActive(false);
    }

    // ------------------- ใส่สร้อย + Spawn Prefab -------------------
    public void Equip(Item necklace)
    {
        equippedNecklace = necklace;

        // ตรวจว่าไอเท็มถูกสีไหม
        if (necklace != null && necklace.itemName == desiredColor)
        { 
            IsCorrect = true; Debug.Log($"{name} ✔ ใส่สีถูกต้องแล้ว"); 
        }
        else 
        {
            IsCorrect = false; Debug.Log($"{name} ✖ สีไม่ถูกต้อง"); 
        }
    }

    // ------------------- เอาสร้อยออก -------------------
    public Item RemoveNecklace()
    {

        Item removed = equippedNecklace;
        equippedNecklace = null;
        IsCorrect = false;

        return removed;
    }
}
