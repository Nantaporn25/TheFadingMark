using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupPopupUI : MonoBehaviour
{
    public static PickupPopupUI instance;

    public GameObject panel;
    public Image itemImage;
    public TextMeshProUGUI itemNameText;

    private bool isOpen = false;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Return))
        {
            Close();
        }
    }

    public void Show(Item item)
    {
        if (item == null) return;

        itemImage.sprite = item.icon != null ? item.icon : item.worldSprite;
        itemNameText.text = item.itemName;

        panel.SetActive(true);
        isOpen = true;

        Time.timeScale = 0f; // กันกดอะไรพลาด
    }

    void Close()
    {
        panel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;
    }

    public bool IsOpen() => isOpen;
}
