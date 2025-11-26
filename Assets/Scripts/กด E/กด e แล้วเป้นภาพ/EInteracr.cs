using UnityEngine;

public class EInteracr : MonoBehaviour
{
    public GameObject uiPanel; // UI Panel ที่จะแสดง
    private bool isPlayerNear = false; // ผู้เล่นอยู่ใกล้หรือไม่
    private bool isUIOpen = false; // UI เปิดอยู่หรือไม่

    void Update()
    {
        if (isPlayerNear)
        {
            // เปิด UI: กด E ตอน UI ยังปิด
            if (!isUIOpen && Input.GetKeyDown(KeyCode.E))
            {
                OpenUI();
            }
            // ปิด UI: กด E หรือ Enter ตอน UI เปิดอยู่
            else if (isUIOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
            {
                CloseUI();
            }
        }
    }

    void OpenUI()
    {
        isUIOpen = true;
        uiPanel.SetActive(true);
        Time.timeScale = 0f; // หยุดเกม
    }

    void CloseUI()
    {
        isUIOpen = false;
        uiPanel.SetActive(false);
        Time.timeScale = 1f; // กลับมาเล่นเกม
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (isUIOpen)
            {
                CloseUI(); // ปิด UI อัตโนมัติถ้าออกจากระยะ
            }
        }
    }
}
