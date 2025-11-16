using UnityEngine;

public class Mannequin : MonoBehaviour
{
    public string desiredColor;
    public Item currentNecklace;

    public bool IsCorrect => currentNecklace != null && currentNecklace.colorName == desiredColor;

    public void EquipNecklace(Item item)
    {
        if (item == null) return;
        currentNecklace = item;
        Debug.Log($"✅ ใส่สร้อย '{item.itemName}' ให้หุ่นที่ต้องการสี {desiredColor}");
    }

    public Item RemoveNecklace()
    {
        Item removed = currentNecklace;
        currentNecklace = null;
        if (removed != null)
            Debug.Log($"❌ เอาสร้อย '{removed.itemName}' ออกจากหุ่น");
        return removed;
    }
}
