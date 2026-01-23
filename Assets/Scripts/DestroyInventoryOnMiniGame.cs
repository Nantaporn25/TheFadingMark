using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyInventoryOnMiniGame : MonoBehaviour
{
    void Start()
    {
        InventoryManager inv = FindObjectOfType<InventoryManager>();
        if (inv != null)
        {
            Destroy(inv.gameObject);
        }
    }
}
