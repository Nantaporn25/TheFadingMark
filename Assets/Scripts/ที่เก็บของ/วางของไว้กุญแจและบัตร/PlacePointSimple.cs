using UnityEngine;

public class PlacePointSimple : MonoBehaviour
{
    private bool playerInRange = false;

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

        // ลบของออกจาก inventory
        inv.leftItems[index] = null;
        inv.UpdateButtonIcon(index);

        Debug.Log("Placed item: " + item.itemName);
    }
}