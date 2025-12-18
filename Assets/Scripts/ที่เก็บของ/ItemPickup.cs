using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;               // ไอเท็มทั่วไป
    public bool isPaper = false;        // กรณีเป็นกระดาษ
    public Sprite[] paperSprites;

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
                    // เพิ่มทุกหน้าเข้า PaperManager
                    foreach (var page in paperSprites)
                    {
                        PaperManager.instance.PickupPaper(page);
                    }

                    Destroy(gameObject);
                }
            }
            // กรณีเป็นไอเท็มทั่วไป
            else if (itemData != null)
            {
                if (InventoryManager.instance != null)
                {
                    bool picked = InventoryManager.instance.PickupItem(itemData);
                    if (picked)
                    {
                        if (PickupPopupUI.instance != null)
                            PickupPopupUI.instance.Show(itemData);

                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
