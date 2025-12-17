using UnityEngine;
using TMPro;

public class CabinetSkillTrigger : MonoBehaviour
{
    private bool playerInRange = false;

    [Header("UI Feedback")]
    public TextMeshProUGUI feedbackText;
    public float showTime = 2f;

    [Header("SkillCheck Reference")]
    public SkillCheckPaperLily skillCheck;

    public void OnSkillSuccess()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "เปิดตู้สำเร็จ!";
            CancelInvoke("ClearFeedback");
            Invoke("ClearFeedback", showTime);
        }
    }

    public void OnSkillFail()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "พลาด! ต้องเริ่มใหม่";
            CancelInvoke("ClearFeedback");
            Invoke("ClearFeedback", showTime);
        }
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
            feedbackText.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (skillCheck != null)
                skillCheck.StartSkillCheck();
        }
    }
}
