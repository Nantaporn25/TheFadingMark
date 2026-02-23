using UnityEngine;

public class HeadQuestManager : MonoBehaviour
{
    public static HeadQuestManager instance;

    [Header("จำนวนหัวที่ต้องวางให้ครบ")]
    public int requiredHeads = 2;

    [Header("Objects ที่จะเปิดเมื่อครบหัว")]
    public GameObject[] rewardObjects;   // 🔥 เปลี่ยนเป็น Array

    private int placedCount = 0;
    private bool alreadyCompleted = false;

    void Update()
    {
        if (!alreadyCompleted && Input.GetKeyDown(KeyCode.K))
        {
            ForceComplete();
        }
    }

    private void Awake()
    {
        instance = this;

        // ปิด reward ทั้งหมดตอนเริ่มเกม
        foreach (GameObject obj in rewardObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    public void AddPlacedHead()
    {
        if (alreadyCompleted) return;

        placedCount++;

        if (placedCount >= requiredHeads)
        {
            CompleteQuest();
        }
    }

    void ForceComplete()
    {
        placedCount = requiredHeads;
        CompleteQuest();
        Debug.Log("🔥 Debug: เปิด reward ทันทีด้วยปุ่ม K");
    }

    void CompleteQuest()
    {
        alreadyCompleted = true;

        foreach (GameObject obj in rewardObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        Debug.Log("✔ ครบ " + requiredHeads + " หัวแล้ว! เปิด reward ทั้งหมด");
    }
}