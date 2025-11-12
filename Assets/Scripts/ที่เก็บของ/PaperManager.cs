using UnityEngine;
using UnityEngine.UI;

public class PaperManager : MonoBehaviour
{
    public static PaperManager instance;

    [Header("UI")]
    public GameObject paperUI;      // Panel กระดาษเต็มจอ
    public Image paperImageUI;      // รูปภาพกระดาษเต็มจอ
    public Image paperIcon;         // ไอคอนเล็กบน UI เกม
    public Sprite defaultPaper;

    private bool hasPaper = false;
    private bool isShowing = false;
    private Sprite currentPaperSprite;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (paperUI != null) paperUI.SetActive(false);
            if (paperIcon != null) paperIcon.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PickupPaper(Sprite paperSprite)
    {
        hasPaper = true;
        currentPaperSprite = paperSprite;
        if (paperIcon != null) paperIcon.enabled = true;
        Debug.Log("📜 เก็บกระดาษแล้ว!");
    }

    void Update()
    {
        if (hasPaper && Input.GetKeyDown(KeyCode.Q))
        {
            if (!isShowing) ShowPaper();
            else ClosePaper();
        }
    }

    void ShowPaper()
    {
        if (paperUI != null && paperImageUI != null)
        {
            paperUI.SetActive(true);
            paperImageUI.sprite = currentPaperSprite != null ? currentPaperSprite : defaultPaper;
            isShowing = true;
            Time.timeScale = 0f;
        }
    }

    void ClosePaper()
    {
        if (paperUI != null)
            paperUI.SetActive(false);
        isShowing = false;
        Time.timeScale = 1f;
    }
}
