using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Mannequin currentMannequin;
    private bool dialogActive = false;

    void Update()
    {
        if (currentMannequin == null) return;

        // --- กด E เพื่อแสดงหรือใส่สร้อย ---
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!dialogActive)
            {
                // แสดงไดอาล็อกก่อน
                currentMannequin.StartDialog();
                dialogActive = true;
            }
            else
            {
                // ถ้าไดอาล็อกถูกปิดไปแล้ว → ใส่สร้อย
                EquipNecklaceToMannequin();
            }
        }

        // --- กด Enter เพื่อปิดไดอาล็อก ---
        if (dialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            currentMannequin.StopDialog();
            dialogActive = false;
        }

        // --- กด R เพื่อนำสร้อยออก ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveNecklaceFromMannequin();
        }
    }

    void EquipNecklaceToMannequin()
    {
        int index = InventoryManager.instance.GetSelectedIndex();
        if (index < 0)
        {
            Debug.Log("ยังไม่ได้เลือกช่อง 1-3");
            return;
        }

        Item item = InventoryManager.instance.leftItems[index];
        if (item == null)
        {
            Debug.Log("ช่องนี้ไม่มีไอเท็ม");
            return;
        }

        currentMannequin.Equip(item);

        InventoryManager.instance.leftItems[index] = null;
        InventoryManager.instance.UpdateButtonIcon(index);

        Debug.Log("✔ ใส่สร้อยให้หุ่นเรียบร้อย");
    }

    void RemoveNecklaceFromMannequin()
    {
        Item removed = currentMannequin.RemoveNecklace();
        if (removed != null)
        {
            InventoryManager.instance.PickupItem(removed);
            Debug.Log("นำสร้อยออกและคืนเข้ากระเป๋าแล้ว");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null)
            currentMannequin = m;
    }

    private void OnTriggerExit(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null && currentMannequin == m)
        {
            currentMannequin.StopDialog();
            currentMannequin = null;
            dialogActive = false;
        }
    }
}
