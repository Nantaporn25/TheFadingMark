using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;               // ไอเท็มทั่วไป
    public bool isPaper = false;        // กรณีเป็นกระดาษ
    public Sprite paperSprite;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // กรณีเป็นกระดาษ
            if (isPaper)
            {
                if (PaperManager.instance != null)
                {
                    PaperManager.instance.PickupPaper(paperSprite);
                    Destroy(gameObject);
                }
            }
            // กรณีเป็นไอเท็มทั่วไป
            else if (itemData != null)
            {
                if (InventoryManager.instance != null)
                {
                    bool picked = InventoryManager.instance.PickupItem(itemData);
                    if (picked) Destroy(gameObject);
                }
            }
        }
    }
}
