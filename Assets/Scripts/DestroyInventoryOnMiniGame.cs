using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyInventoryOnMiniGame : MonoBehaviour
{
    void Start()
    {
        InventoryManager inv = FindFirstObjectByType<InventoryManager>();
        if (inv != null)
        {
            Destroy(inv.gameObject);
        }
    }
}