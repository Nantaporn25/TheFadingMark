using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;  // ชื่อ NPC / Player
    public Sprite characterImage; // รูปตัวละคร
    [TextArea(2, 5)]
    public string sentence;       // ข้อความที่จะพูด
}