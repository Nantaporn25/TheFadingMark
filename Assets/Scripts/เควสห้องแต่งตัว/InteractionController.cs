using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    private bool dialogActive = false;
    public List<Mannequin> mannequinsInRange = new List<Mannequin>();

    void Update()
    {
        // --- กด C เพื่อคุย / ใส่สร้อย ---
        if (Input.GetKeyDown(KeyCode.C))
        {
            Mannequin target = GetClosestMannequin();
            if (target == null) return;

            if (!dialogActive)
            {
                target.StartDialog();
                dialogActive = true;
            }
            else
            {
                EquipNecklaceToMannequin(target);
                dialogActive = false;
                target.StopDialog();
            }
        }

        // --- กด Enter เพื่อปิดไดอะล็อก ---
        if (dialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            Mannequin target = GetClosestMannequin();
            if (target != null) target.StopDialog();
            dialogActive = false;
        }

        // --- กด R เพื่อถอดสร้อย ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            Mannequin target = GetClosestMannequin();
            if (target != null) RemoveNecklaceFromMannequin(target);
        }
    }

    // ------------------- ใส่สร้อย -------------------
    void EquipNecklaceToMannequin(Mannequin target)
    {
        int index = InventoryManager.instance.GetSelectedIndex();
        if (index < 0) return;

        Item original = InventoryManager.instance.leftItems[index];
        if (original == null) return;

        // ⭐ Clone item ก่อนส่งให้หุ่น
        Item copy = original.Clone();

        target.Equip(copy);

        // ลบของใน inventory
        InventoryManager.instance.leftItems[index] = null;
        InventoryManager.instance.UpdateButtonIcon(index);

        Debug.Log($"✔ ใส่สร้อยให้ {target.characterName}: {copy.colorName}");
    }

    // ------------------- ถอดสร้อย -------------------
    void RemoveNecklaceFromMannequin(Mannequin target)
    {
        Item removed = target.RemoveNecklace();
        if (removed != null) InventoryManager.instance.PickupItem(removed);
    }

    // ------------------- หาเป้าหมายใกล้ที่สุด -------------------
    private Mannequin GetClosestMannequin()
    {
        float minDist = float.MaxValue;
        Mannequin closest = null;

        foreach (var m in mannequinsInRange)
        {
            if (m == null) continue;
            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = m;
            }
        }

        return closest;
    }

    // ------------------- Trigger 2D -------------------
    private void OnTriggerEnter2D(Collider2D other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null && !mannequinsInRange.Contains(m))
            mannequinsInRange.Add(m);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null)
        {
            mannequinsInRange.Remove(m);
            m.StopDialog();
            dialogActive = false;
        }
    }
}
