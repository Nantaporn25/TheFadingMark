using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [Header("UI References (Inspector เชื่อม)")]
    public Button[] leftButtons;
    public Image[] leftHighlights;
    public Image[] leftIcons;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [Header("Inventory Data")]
    public Item[] leftItems; // 3 ช่องเก็บของ

    private int selectedIndex = -1;

    public int placedItemCount = 0; //โค้ดใหม่

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (leftItems == null || leftItems.Length != 3)
                leftItems = new Item[3];

            SceneManager.sceneLoaded += OnSceneLoaded;
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

    //โค้ดใหม่
    void Update()
    {
        // ตรวจสอบการกดปุ่ม 1-3 เพื่อเลือกช่องในกระเป๋า
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectButton(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectButton(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectButton(2);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIReferences();
        InitializeUI();
    }

    void FindUIReferences()
    {
        // หาปุ่ม + ไฮไลต์ + ไอคอน ด้วย Tag
        if (leftButtons == null || leftButtons.Length == 0)
            leftButtons = GameObject.FindGameObjectsWithTag("InvButton").Select(g => g.GetComponent<Button>()).ToArray();
        if (leftHighlights == null || leftHighlights.Length == 0)
            leftHighlights = GameObject.FindGameObjectsWithTag("InvHighlight").Select(g => g.GetComponent<Image>()).ToArray();
        if (leftIcons == null || leftIcons.Length == 0)
            leftIcons = GameObject.FindGameObjectsWithTag("InvIcon").Select(g => g.GetComponent<Image>()).ToArray();

        if (dialoguePanel == null)
            dialoguePanel = GameObject.Find("DialoguePanel");
        if (dialoguePanel != null && dialogueText == null)
            dialogueText = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    void InitializeUI()
    {
        if (leftHighlights != null)
            foreach (var h in leftHighlights)
                if (h != null) h.enabled = false;

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
        if (leftHighlights != null)
        {
            for (int i = 0; i < leftHighlights.Length; i++)
                if (leftHighlights[i] != null)
                    leftHighlights[i].enabled = (i == selectedIndex);
        }
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
            Debug.Log(string.IsNullOrEmpty(leftItems[i].itemName));

            if (string.IsNullOrEmpty(leftItems[i].itemName)) //!!!!!!
            {
                leftItems[i] = newItem;

                // leftItems[i] = null; remove item

                UpdateButtonIcon(i);
                Debug.Log($"✅ เก็บไอเท็ม '{newItem.itemName}' ลงช่อง {i + 1}");
                return true;
            }
        }

        // ช่องเต็ม → แสดง Dialogue
        if (dialoguePanel != null && dialogueText != null)
        {
            dialogueText.text = "เหมือนมือถือจะแจ้งเตือนแฮะ";
            dialoguePanel.SetActive(true);
        }

        return false;
    }
}