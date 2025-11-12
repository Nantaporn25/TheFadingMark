using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI References")]
    public Button[] leftButtons;
    public Image[] leftIcons;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Inventory Data")]
    public Item[] leftItems; // 3 ช่องเก็บของ

    private int selectedIndex = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (leftItems == null || leftItems.Length != 3)
                leftItems = new Item[3];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        if (leftButtons != null)
        {
            for (int i = 0; i < leftButtons.Length; i++)
            {
                int index = i;
                leftButtons[i].onClick.RemoveAllListeners();
                leftButtons[i].onClick.AddListener(() => SelectButton(index));
                UpdateButtonIcon(i);
            }
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void SelectButton(int index)
    {
        selectedIndex = index;
    }

    void UpdateButtonIcon(int index)
    {
        if (leftIcons == null || leftIcons.Length <= index) return;
        if (leftItems != null && leftItems.Length > index && leftItems[index] != null)
            leftIcons[index].sprite = leftItems[index].icon;
        else
            leftIcons[index].sprite = null;
    }

    public bool PickupItem(Item newItem)
    {
        if (newItem == null) return false;

        for (int i = 0; i < leftItems.Length; i++)
        {
            if (leftItems[i] == null || string.IsNullOrEmpty(leftItems[i].itemName))
            {
                leftItems[i] = newItem;
                UpdateButtonIcon(i);
                Debug.Log($"✅ เก็บไอเท็ม '{newItem.itemName}' ลงช่อง {i + 1}");
                return true;
            }
        }

        if (dialoguePanel != null && dialogueText != null)
        {
            dialogueText.text = "Inventory เต็มแล้ว!";
            dialoguePanel.SetActive(true);
        }

        return false;
    }
}
