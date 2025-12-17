using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCheckPaperLily : MonoBehaviour
{
    [Header("UI Elements")]
    public Image backgroundPanel;
    public Image barBackground;
    public RectTransform pointer;
    public RectTransform targetZone;
    public Image progressCircleFill;
    public Image progressCircleOutline;
    public TextMeshProUGUI feedbackText;

    [Header("Settings")]
    public float moveSpeed = 300f;
    public int requiredSuccessInRow = 3;

    [Header("References")]
    public CabinetSkillTrigger[] cabinets;

    [Header("After Skill Dialog")]
    public AfterSkill afterSkill;
    public int dialogIndex = 0;

    private bool canPressKey = false;
    private bool isMovingRight = true;
    private float pointerPos = 0f;
    private float barWidth;
    private Vector3 startPos;
    private Vector3 endPos;
    private int currentStreak = 0;

    void Start()
    {
        gameObject.SetActive(false);
        if (backgroundPanel) backgroundPanel.gameObject.SetActive(false);
        if (feedbackText) feedbackText.text = "กด Enter เพื่อทำให้ครบ";
    }

    void Update()
    {
        if (!canPressKey) return;

        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            CheckSkillResult();
        }
    }

    public void StartSkillCheck()
    {
        gameObject.SetActive(true);
        if (backgroundPanel) backgroundPanel.gameObject.SetActive(true);
        if (feedbackText) feedbackText.text = "กด Enter เพื่อทำให้ครบ";

        canPressKey = true;
        currentStreak = 0;
        progressCircleFill.fillAmount = 0f;

        barWidth = barBackground.rectTransform.rect.width;
        startPos = barBackground.rectTransform.localPosition - new Vector3(barWidth / 2f, 0f, 0f);
        endPos = barBackground.rectTransform.localPosition + new Vector3(barWidth / 2f, 0f, 0f);

        pointerPos = 0f;
        isMovingRight = true;

        RandomizeTargetZone();
    }

    void MovePointer()
    {
        pointerPos += (isMovingRight ? 1 : -1) * moveSpeed * Time.deltaTime / barWidth;

        if (pointerPos >= 1f) { pointerPos = 1f; isMovingRight = false; }
        if (pointerPos <= 0f) { pointerPos = 0f; isMovingRight = true; }

        pointer.localPosition = Vector3.Lerp(startPos, endPos, pointerPos);
    }

    void CheckSkillResult()
    {
        float dist = Mathf.Abs(pointer.localPosition.x - targetZone.localPosition.x);
        if (dist <= targetZone.rect.width / 2f)
            OnSuccess();
        else
            OnFail();
    }

    void OnSuccess()
    {
        currentStreak++;
        progressCircleFill.fillAmount = (float)currentStreak / requiredSuccessInRow;

        if (feedbackText) feedbackText.text = "ถูก !";

        if (currentStreak >= requiredSuccessInRow)
        {
            FinishSkillCheck();

            // เรียก Cabinet ทุกตัว
            if (cabinets != null)
            {
                foreach (var cab in cabinets)
                    if (cab != null) cab.OnSkillSuccess();
            }

            return;
        }

        RandomizeTargetZone();
    }

    void OnFail()
    {
        currentStreak = 0;
        progressCircleFill.fillAmount = 0f;

        if (feedbackText) feedbackText.text = "พลาด ! ต้องเริ่มใหม่";

        if (cabinets != null)
        {
            foreach (var cab in cabinets)
                if (cab != null) cab.OnSkillFail();
        }

        RandomizeTargetZone();
    }

    void FinishSkillCheck()
    {
        canPressKey = false;
        gameObject.SetActive(false);
        if (backgroundPanel) backgroundPanel.gameObject.SetActive(false);
        if (feedbackText) feedbackText.text = "";

        // เรียก AfterSkill Dialog
        if (afterSkill != null)
        {
            afterSkill.ShowDialog(dialogIndex);
        }
    }

    void RandomizeTargetZone()
    {
        float range = barWidth * 0.8f;
        float randomX = Random.Range(-range / 2f, range / 2f);
        Vector3 pos = targetZone.localPosition;
        pos.x = randomX;
        targetZone.localPosition = pos;
    }
}
