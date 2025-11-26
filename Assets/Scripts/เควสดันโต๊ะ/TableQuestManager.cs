using UnityEngine;

public class TableQuestManager : MonoBehaviour
{
    public GridPushableTable[] tables;

    private bool questCompleted = false;

    [Header("Dialog เมื่อเควสเสร็จ")]
    [TextArea(2, 5)]
    public string[] completeDialog = {
        "สุดยอด! โต๊ะทั้งหมดถูกวางอย่างถูกต้องแล้ว!",
        "ต่อไปเรามีงานอื่นให้ทำอีกนะ!"
    };

    [Header("Reward Object (วางไว้ในแมพแล้วปิดไว้ก่อน)")]
    public GameObject rewardObject;

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

        // ✨ ให้รางวัลปรากฎในแมพ
        ShowReward();

        // ✨ เปิดไดอาล็อกเควสเสร็จ
        QuestDialog.instance.lines = completeDialog;
        QuestDialog.instance.StartDialog();
    }

    private void ShowReward()
    {
        if (rewardObject != null)
        {
            rewardObject.SetActive(true);
            Debug.Log("Reward appeared on the map!");
        }
    }
}
