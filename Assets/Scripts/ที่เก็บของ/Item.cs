using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;   // ชื่อไอเท็ม
    public Sprite icon;       // ไอคอน
    public string colorName;  // สีที่ใช้เช็ค

    public Item Clone()
    {
        return new Item
        {
            itemName = this.itemName,
            icon = this.icon,
            colorName = this.colorName
        };
    }
}
