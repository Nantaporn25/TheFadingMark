using UnityEngine;

public class MainMannequin : MonoBehaviour
{
    public Mannequin[] mannequins; // 5 ตัว

    public void CheckAllMannequins()
    {
        foreach (var m in mannequins)
        {
            if (!m.IsCorrect)
            {
                Debug.Log("❌ ใส่ผิด มีหุ่นอย่างน้อย 1 ตัวไม่ถูกต้อง");
                return;
            }
        }

        Debug.Log("🎉 ใส่ถูกต้องทั้งหมด! ให้รางวัลผู้เล่น");
        GiveReward();
    }

    void GiveReward()
    {
        // ใส่โค้ดให้รางวัล เช่น item ใหม่, achievement, dialog ฯลฯ
    }
}
