using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    public int targetBuildIndex = 2;

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
            TryPlaceKeyNecklace();
    }

    void TryPlaceKeyNecklace()
    {
        InventoryManager inv = InventoryManager.instance;
        if (inv == null) return;

        int index = inv.GetSelectedIndex();
        if (index < 0) return;

        Item item = inv.leftItems[index];
        if (item == null) return;

        if (item.itemName != "keyNecklace") return;

        // เอาของออก
        inv.leftItems[index] = null;
        inv.UpdateButtonIcon(index);

        // Fade → เปลี่ยนซีน
        FadeSceneManager.instance.FadeToScene(targetBuildIndex);
    }
}
