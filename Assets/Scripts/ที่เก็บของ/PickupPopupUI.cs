using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupPopupUI : MonoBehaviour
{
    public static PickupPopupUI instance;

    [Header("UI")]
    public GameObject panel;
    public Image itemImage;
    public TextMeshProUGUI itemNameText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip popupSFX;

    [Header("Next System")]
    public SimpleDialogueUI dialogueUI;

    private bool isOpen = false;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);

        // ให้เสียงดังแม้ Time.timeScale = 0
        if (audioSource != null)
            audioSource.ignoreListenerPause = true;
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

        // 🔊 เล่นเสียง Popup 1 ครั้ง
        if (audioSource != null && popupSFX != null)
        {
            audioSource.PlayOneShot(popupSFX);
        }

        Time.timeScale = 0f;
    }

    void Close()
    {
        panel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;

        // ต่อ Dialogue หลัง Popup
        if (dialogueUI != null)
        {
            dialogueUI.StartDialogue();
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
