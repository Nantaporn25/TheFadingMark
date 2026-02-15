using UnityEngine;

public class HeadQuestManager : MonoBehaviour
{
    public static HeadQuestManager instance;

    [Header("จำนวนหัวที่ต้องวางให้ครบ")]
    public int requiredHeads = 2;   // ตั้งค่าได้ใน Inspector (2 โดยค่าเริ่มต้น)

    [Header("Object ที่จะเปิดเมื่อครบหัว")]
    public GameObject rewardObject;

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
        rewardObject.SetActive(false);
    }

    public void AddPlacedHead()
    {
        if (alreadyCompleted) return;

        placedCount++;

        if (placedCount >= requiredHeads)
        {
            alreadyCompleted = true;

            if (rewardObject != null)
            {
                rewardObject.SetActive(true);
                Debug.Log("✔ ครบ " + requiredHeads + " หัวแล้ว! เปิด reward");
            }
        }
    }
    void ForceComplete()
    {
        alreadyCompleted = true;
        placedCount = requiredHeads;   // ทำให้ครบตามเงื่อนไข

        if (rewardObject != null)
        {
            rewardObject.SetActive(true);
            Debug.Log("🔥 Debug: เปิด reward ทันทีด้วยปุ่ม K");
        }
    }
}
