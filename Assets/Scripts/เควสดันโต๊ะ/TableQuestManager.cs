using UnityEngine;

public class TableQuestManager : MonoBehaviour
{
    public GridPushableTable[] tables; // โต๊ะทั้งหมดในฉาก

    private bool questCompleted = false;

    void Update()
    {
        if (questCompleted) return; // เควสแล้วเสร็จแล้ว

        bool allPlaced = true;

        foreach (var table in tables)
        {
            if (!table.isAtTarget)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            questCompleted = true;
            Debug.Log("เควสสำเร็จ! โต๊ะทั้งหมดวางถูกตำแหน่งแล้ว");
            // สามารถเรียกฟังก์ชันอื่นต่อ เช่น เปิดประตู, ให้รางวัล ฯลฯ
        }
    }
}
