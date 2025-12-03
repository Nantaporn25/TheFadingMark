using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject phoneUIFullScreen; // UI มือถือเต็มจอ
    public Button[] appButtons;           // ปุ่มแอป 2 ปุ่ม
    public Image[] appHighlights;         // ไฮไลต์รอบปุ่ม

    [Header("App UIs")]
    public GameObject facebookUIFullScreen; // UI Facebook (รูปภาพเต็มจอ)
    public GameObject lineUIFullScreen;     // UI LINE (รูปภาพเต็มจอ)

    private bool isPhoneOpen = false;
    private int selectedAppIndex = -1;    // -1 = ยังไม่เลือก
    private bool isFacebookUIOpen = false;
    private bool isLineUIOpen = false;

    void Start()
    {
        if (phoneUIFullScreen != null)
            phoneUIFullScreen.SetActive(false);

        if (facebookUIFullScreen != null)
            facebookUIFullScreen.SetActive(false);

        if (lineUIFullScreen != null)
            lineUIFullScreen.SetActive(false);

        // ปิดไฮไลต์ตอนเริ่ม
        foreach (var h in appHighlights)
            h.enabled = false;

        // เชื่อมปุ่มแอปกับฟังก์ชัน
        for (int i = 0; i < appButtons.Length; i++)
        {
            int index = i;
            appButtons[i].onClick.AddListener(() => OpenApp(index));
        }
    }

    void Update()
    {
        // เปิด/ปิดมือถือด้วย X
        if (Input.GetKeyDown(KeyCode.X))
            TogglePhoneUI();

        if (!isPhoneOpen) return;

        // เลือกแอปด้วยเลข 1–2
        for (int i = 0; i < appButtons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedAppIndex = i;
                UpdateHighlights();
            }
        }

        // กด Enter → เปิด/ปิดแอปตามที่เลือก
        if (selectedAppIndex == 0 && Input.GetKeyDown(KeyCode.Return))
            ToggleFacebookUI();

        if (selectedAppIndex == 1 && Input.GetKeyDown(KeyCode.Return))
            ToggleLineUI();
    }

    void TogglePhoneUI()
    {
        if (phoneUIFullScreen == null) return;

        isPhoneOpen = !isPhoneOpen;
        phoneUIFullScreen.SetActive(isPhoneOpen);
        Time.timeScale = isPhoneOpen ? 0f : 1f;

        if (!isPhoneOpen)
        {
            // ปิดไฮไลต์
            foreach (var h in appHighlights)
                h.enabled = false;
            selectedAppIndex = -1;

            // ปิด Facebook และ LINE UI
            if (facebookUIFullScreen != null)
                facebookUIFullScreen.SetActive(false);
            if (lineUIFullScreen != null)
                lineUIFullScreen.SetActive(false);
            isFacebookUIOpen = false;
            isLineUIOpen = false;
        }
    }

    void UpdateHighlights()
    {
        for (int i = 0; i < appHighlights.Length; i++)
            appHighlights[i].enabled = (i == selectedAppIndex);
    }

    void ToggleFacebookUI()
    {
        if (facebookUIFullScreen == null) return;
        isFacebookUIOpen = !isFacebookUIOpen;
        facebookUIFullScreen.SetActive(isFacebookUIOpen);
    }

    void ToggleLineUI()
    {
        if (lineUIFullScreen == null) return;
        isLineUIOpen = !isLineUIOpen;
        lineUIFullScreen.SetActive(isLineUIOpen);
    }

    void OpenApp(int index)
    {
        // สามารถกดคลิกด้วยเมาส์ก็เปิดได้
        if (index == 0) Debug.Log("Facebook (ใช้ Enter เปิด/ปิด UI)");
        if (index == 1) Debug.Log("LINE (ใช้ Enter เปิด/ปิด UI)");
    }
}
