using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;
    public bool isPaper = false;
    public Sprite paperSprite;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isPaper)
            {
                if (PaperManager.instance != null)
                {
                    PaperManager.instance.PickupPaper(paperSprite);
                    Destroy(gameObject);
                }
            }
            else
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
