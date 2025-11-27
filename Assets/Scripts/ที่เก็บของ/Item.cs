using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;    // ชื่อไอเท็ม
    public Sprite icon;        // ไอคอน UI ช่องเก็บ
    public string colorName;   // สีของสร้อย
    public Sprite worldSprite; // ไอคอนที่จะไปแสดงบนหุ่น / โลกจริง

    public Item Clone()
    {
        return new Item
        {
            itemName = this.itemName,
            icon = this.icon,
            colorName = this.colorName,
            worldSprite = this.worldSprite
        };
    }
}
