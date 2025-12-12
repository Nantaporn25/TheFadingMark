using UnityEngine;

public class HeadPlacementSpot : MonoBehaviour
{
    [Header("ข้อมูลหัวที่ถูกวาง")]
    public Item placedHead;
    private GameObject headObject;

    [Header("ตำแหน่งวางหัว")]
    public Transform headPoint;

    private bool playerNear = false;

    void Update()
    {
        if (!playerNear) return;

        // ------------------ กด E เพื่อวางหัว ------------------
        if (Input.GetKeyDown(KeyCode.E))
        {
            int index = InventoryManager.instance.GetSelectedIndex();

            if (index < 0)
            {
                Debug.Log("ยังไม่ได้เลือกช่อง Inventory (กด 1-3)");
                return;
            }

            Item item = InventoryManager.instance.leftItems[index];

            if (item == null)
            {
                Debug.Log("ช่องนี้ไม่มีไอเท็ม");
                return;
            }

            // วางหัวลงตำแหน่งนี้
            PlaceHead(item);

            // ลบไอเท็มออกจาก inventory
            InventoryManager.instance.leftItems[index] = null;
            InventoryManager.instance.UpdateButtonIcon(index);

            Debug.Log("✔ วางหัวสำเร็จ: " + item.itemName);
        }
    }

    // ------------------ ฟังก์ชันวางหัว ------------------
    public void PlaceHead(Item head)
    {
        placedHead = head;

        if (headObject != null)
            Destroy(headObject);

        if (head == null || head.worldSprite == null)
        {
            Debug.LogWarning("ไอเท็มนี้ไม่มี worldSprite!");
            return;
        }

        headObject = new GameObject("PlacedHead");
        SpriteRenderer sr = headObject.AddComponent<SpriteRenderer>();
        sr.sprite = head.worldSprite;
        sr.sortingOrder = 20;

        headObject.transform.SetParent(headPoint);
        headObject.transform.localPosition = Vector3.zero;

        HeadQuestManager.instance.AddPlacedHead();
    
}

    // ------------------ เข้าพื้นที่ ------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    // ------------------ ออกจากพื้นที่ ------------------
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
