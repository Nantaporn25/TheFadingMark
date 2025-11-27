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
    public string desiredColor;
    public Item equippedNecklace;

    [Header("สถานะเควส")]
    public bool IsCorrect = false;

    [Header("UI อ้างอิง")]
    public GameObject dialogBox;
    public Image profileImageUI;
    public TextMeshProUGUI nameTextUI;
    public TextMeshProUGUI dialogTextUI;

    [Header("ตำแหน่งติดสร้อยในโลกเกม")]
    public Transform necklacePoint;
    private GameObject necklaceObject;

    private int dialogIndex = 0;

    // ------------------------- ไดอะล็อก -------------------------
    public void StartDialog()
    {
        if (dialogLines == null || dialogLines.Length == 0) return;

        dialogBox.SetActive(true);

        if (profileImageUI != null) profileImageUI.sprite = profileImage;
        if (nameTextUI != null) nameTextUI.text = characterName;

        dialogIndex = 0;
        dialogTextUI.text = dialogLines[dialogIndex];
    }

    public void StopDialog()
    {
        dialogBox.SetActive(false);
    }

    // ------------------- ใส่สร้อย -------------------
    public void Equip(Item necklace)
    {
        equippedNecklace = necklace;

        // ⭐ ใส่สร้อยบนหุ่นไม่จำเป็นต้องถูกสี
        SetNecklace(necklace);

        // เช็คสีสำหรับสถานะเควส
        if (necklace != null && necklace.colorName == desiredColor)
        {
            IsCorrect = true;
            Debug.Log($"{name} ✔ สีถูกต้อง");
        }
        else
        {
            IsCorrect = false;
            Debug.Log($"{name} ✖ สีไม่ถูกต้อง");
        }
    }

    // ------------------- ถอดสร้อย -------------------
    public Item RemoveNecklace()
    {
        Item removed = equippedNecklace;
        equippedNecklace = null;
        IsCorrect = false;

        if (necklaceObject != null)
        {
            Destroy(necklaceObject);
            necklaceObject = null;
        }

        return removed;
    }

    // ------------------- แสดงสร้อยบนหุ่น -------------------
    public void SetNecklace(Item item)
    {
        if (necklaceObject != null)
            Destroy(necklaceObject);

        if (item == null || item.worldSprite == null) return;

        necklaceObject = new GameObject("NecklaceVisual");
        SpriteRenderer sr = necklaceObject.AddComponent<SpriteRenderer>();
        sr.sprite = item.worldSprite;  // ⭐ ใช้ worldSprite แทน icon
        sr.sortingOrder = 10;

        necklaceObject.transform.SetParent(necklacePoint);
        necklaceObject.transform.localPosition = Vector3.zero;
    }

    // ------------------- เปลี่ยนไอคอนสร้อยบนหุ่น -------------------
    public void UpdateNecklaceIcon(Sprite newIcon)
    {
        if (necklaceObject == null)
        {
            Debug.LogWarning("ยังไม่มีสร้อยบนหุ่น!");
            return;
        }

        SpriteRenderer sr = necklaceObject.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = newIcon;
    }
}
