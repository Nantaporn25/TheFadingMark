using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("UI & Interaction")]
    private bool dialogActive = false;

    [Header("หุ่นที่อยู่ใกล้ผู้เล่น")]
    public List<Mannequin> mannequinsInRange = new List<Mannequin>();

    void Update()
    {
        // --- กด C เพื่อแสดงไดอะล็อกหรือใส่สร้อย ---
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

                // ตรวจสอบหุ่นทุกตัวผ่าน MainMannequin (เวอร์ชันใหม่)
                MainMannequin main = Object.FindFirstObjectByType<MainMannequin>();
                if (main != null)
                    main.CheckAllMannequins();
            }
        }

        // --- กด Enter เพื่อปิดไดอะล็อก ---
        if (dialogActive && Input.GetKeyDown(KeyCode.Return))
        {
            Mannequin target = GetClosestMannequin();
            if (target != null)
                target.StopDialog();

            dialogActive = false;
        }

        // --- กด R เพื่อนำสร้อยออก ---
        if (Input.GetKeyDown(KeyCode.R))
        {
            Mannequin target = GetClosestMannequin();
            if (target != null)
                RemoveNecklaceFromMannequin(target);
        }
    }

    // ----------------- ใส่สร้อย -----------------
    void EquipNecklaceToMannequin(Mannequin target)
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

        target.Equip(item);

        InventoryManager.instance.leftItems[index] = null;
        InventoryManager.instance.UpdateButtonIcon(index);

        Debug.Log($"✔ ใส่สร้อยให้ {target.characterName} เรียบร้อย");
    }

    // ----------------- เอาสร้อยออก -----------------
    void RemoveNecklaceFromMannequin(Mannequin target)
    {
        Item removed = target.RemoveNecklace();
        if (removed != null)
        {
            InventoryManager.instance.PickupItem(removed);
            Debug.Log($"นำสร้อยออกจาก {target.characterName} และคืนเข้ากระเป๋าแล้ว");
        }
    }

    // ----------------- เลือกหุ่นใกล้ผู้เล่นที่สุด -----------------
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

    // ----------------- Trigger เข้า/ออก -----------------
    private void OnTriggerEnter(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null && !mannequinsInRange.Contains(m))
            mannequinsInRange.Add(m);
    }

    private void OnTriggerExit(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null)
        {
            mannequinsInRange.Remove(m);
            m.StopDialog();
            if (dialogActive)
                dialogActive = false;
        }
    }
}
