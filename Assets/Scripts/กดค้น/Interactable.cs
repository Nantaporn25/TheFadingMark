using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string itemName;
    public Sprite icon; // ไอคอนใน Inventory
    public GameObject alertIcon; // ไอคอนตกใจบนหัว Player

    [HideInInspector] public bool isPlayerNearby = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (alertIcon != null) alertIcon.SetActive(true); // แสดงไอคอนตกใจ
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (alertIcon != null) alertIcon.SetActive(false); // ปิดไอคอนตกใจ
        }
    }

    public void Interact()
    {
        if (!isPlayerNearby) return;

        // เก็บของเข้า Inventory
        InventoryManager.instance.PickupItem(new Item { itemName = itemName, icon = icon });

        // ลบวัตถุออกจาก Scene
        Destroy(gameObject);
    }
}
