using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PaperManager : MonoBehaviour
{
    public static PaperManager instance;

    [Header("UI")]
    public GameObject paperUI;
    public Image paperImageUI;
    public Image paperIcon;
    public Sprite defaultPaper;

    [Header("Page Number UI")]
    public TextMeshProUGUI pageNumberText;   // <--- เพิ่มตัวเลขหน้า

    private List<Sprite> paperPages = new List<Sprite>();
    private int currentPage = 0;

    private bool isShowing = false;

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
        paperPages.Add(paperSprite);

        if (paperIcon != null)
            paperIcon.enabled = true;

        Debug.Log("📜 เก็บกระดาษหน้า " + paperPages.Count);
    }

    void Update()
    {
        if (paperPages.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isShowing) ShowPaper();
            else ClosePaper();
        }

        if (isShowing)
        {
            if (Input.GetKeyDown(KeyCode.D)) NextPage();
            if (Input.GetKeyDown(KeyCode.A)) PrevPage();
        }
    }

    void ShowPaper()
    {
        if (paperUI != null && paperImageUI != null)
        {
            paperUI.SetActive(true);

            currentPage = 0;
            UpdatePage();

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

    void NextPage()
    {
        if (currentPage < paperPages.Count - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        if (paperImageUI != null)
        {
            Sprite s = paperPages.Count > 0 ? paperPages[currentPage] : defaultPaper;
            paperImageUI.sprite = s;
        }

        // --- อัปเดตหมายเลขหน้า ---
        if (pageNumberText != null)
        {
            pageNumberText.text = $"Page {currentPage + 1} / {paperPages.Count}";
        }
    }
}
