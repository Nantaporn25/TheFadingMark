using UnityEngine;

public class MainMannequin : MonoBehaviour
{
    public Mannequin[] mannequins; // หุ่นทั้งหมด

    public void CheckAllMannequins()
    {
        foreach (var m in mannequins)
        {
            if (!m.IsCorrect)
            {
                Debug.Log("❌ มีหุ่นอย่างน้อย 1 ตัวใส่สีผิด");
                return;
            }
        }

        Debug.Log("🎉 ถูกต้องทั้งหมด! ปล่อยของรางวัล");
        GiveReward();
    }

    void GiveReward()
    {
        // ทำอะไรก็ว่าไป เช่น เปิดประตู, item ใหม่
    }
}
