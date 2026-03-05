using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    public int targetBuildIndex = 2;

    [Header("Sound")]
    public AudioClip placeSound;
    private AudioSource audioSource;

    private bool playerInRange = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
            TryPlaceItem();
    }

    void TryPlaceItem()
    {
        InventoryManager inv = InventoryManager.instance;
        if (inv == null) return;

        int index = inv.GetSelectedIndex();
        if (index < 0) return;

        Item item = inv.leftItems[index];
        if (item == null) return;

        // ✅ อนุญาต 2 ชิ้นนี้
        if (item.itemName != "เสื้อริน" && item.itemName != "บัตรนักเรียน")
            return;

        // เอาของออก
        inv.leftItems[index] = null;
        inv.UpdateButtonIcon(index);

        // 🔊 เล่นเสียงวาง
        if (placeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(placeSound);
        }

        // เปลี่ยนซีน
        FadeSceneManager.instance.FadeToScene(targetBuildIndex);
    }
}