using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TableInteract : MonoBehaviour
{
    private bool playerNearby = false;

    // ⭐ เก็บชื่อ Picture Scraps ที่ใส่ไปแล้ว (กันใส่ซ้ำ)
    private HashSet<string> placedPictures = new HashSet<string>();

    [Header("Scene Change")]
    [Tooltip("ฉากที่จะเปลี่ยนไปเมื่อใส่ Picture Scraps ครบ 3 ชิ้น")]
    public string nextSceneName;

    void Update()
    {
        if (!playerNearby) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryManager.instance == null) return;

            int index = InventoryManager.instance.GetSelectedIndex();
            if (index < 0) return;

            Item item = InventoryManager.instance.leftItems[index];
            if (item == null) return;

            // ⭐ เช็กว่าเป็น Picture Scraps
            if (!item.itemName.StartsWith("Picture Scraps"))
                return;

            // ⭐ กันใส่ซ้ำ
            if (placedPictures.Contains(item.itemName))
                return;

            // ลบของออกจาก Inventory
            InventoryManager.instance.RemoveSelectedItem();

            placedPictures.Add(item.itemName);
            Debug.Log("ใส่แล้ว: " + item.itemName);

            // ⭐ ครบ 3 ชิ้น → เปลี่ยนฉาก
            if (placedPictures.Count >= 3 && !string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}
