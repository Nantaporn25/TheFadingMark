using UnityEngine;

public class TableQuestManager : MonoBehaviour
{
    public GridPushableTable[] tables;

    private bool questCompleted = false;

    public string[] completeDialog = {
        "สุดยอด! โต๊ะทั้งหมดถูกวางอย่างถูกต้องแล้ว!",
        "ต่อไปเรามีงานอื่นให้ทำอีกนะ!"
    };

    void Update()
    {
        if (questCompleted) return;

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
            TriggerQuestComplete();
        }
    }

    private void TriggerQuestComplete()
    {
        Debug.Log("Quest Completed!");

        // เรียกแสดงไดอะล็อก
        QuestDialog.instance.StartDialog();
    }
}
