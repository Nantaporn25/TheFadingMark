using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Current Target")]
    public Mannequin currentMannequin; // หุ่นที่ Player อยู่ใกล้
    private bool dialogShown = false;

    void Update()
    {
        if (currentMannequin == null) return;

        // กด E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogShown)
            {
                ShowDialog();
            }
            else
            {
                EquipNecklaceToMannequin();
                dialogShown = false;
            }
        }

        // กด R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveNecklaceFromMannequin();
        }
    }

    #region Dialog & Equip
    void ShowDialog()
    {
        if (InventoryManager.instance.dialoguePanel != null &&
            InventoryManager.instance.dialogueText != null)
        {
            InventoryManager.instance.dialogueText.text =
                $"หุ่นต้องการสี {currentMannequin.desiredColor}";
            InventoryManager.instance.dialoguePanel.SetActive(true);
        }
        dialogShown = true;
    }

    void EquipNecklaceToMannequin()
    {
        int index = InventoryManager.instance.GetSelectedIndex();
        if (index < 0) return;

        Item item = InventoryManager.instance.leftItems[index];
        if (item == null) return;

        currentMannequin.EquipNecklace(item);

        // เอาไอเท็มออกจาก inventory
        InventoryManager.instance.leftItems[index] = null;
        InventoryManager.instance.UpdateButtonIcon(index);

        // ปิด Dialog หลังใส่สร้อย
        if (InventoryManager.instance.dialoguePanel != null)
            InventoryManager.instance.dialoguePanel.SetActive(false);
    }
    #endregion

    #region Remove Necklace
    void RemoveNecklaceFromMannequin()
    {
        Item removed = currentMannequin.RemoveNecklace();
        if (removed != null)
        {
            InventoryManager.instance.PickupItem(removed);
        }
    }
    #endregion

    #region Trigger Detection
    private void OnTriggerEnter(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null)
        {
            currentMannequin = m;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Mannequin m = other.GetComponent<Mannequin>();
        if (m != null && currentMannequin == m)
        {
            currentMannequin = null;

            // ปิด Dialog ถ้าออกจากระยะ
            if (InventoryManager.instance.dialoguePanel != null)
                InventoryManager.instance.dialoguePanel.SetActive(false);

            dialogShown = false;
        }
    }
    #endregion
}
