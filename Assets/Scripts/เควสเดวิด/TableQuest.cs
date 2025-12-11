using UnityEngine;

public class TableQuest : MonoBehaviour
{
    public Transform[] slotPoints;
    public GameObject[] headPrefabs;
    public GameObject rewardImage;

    private int headsPlaced = 0;
    private bool playerNear = false;

    private void Start()
    {
        if (rewardImage != null)
            rewardImage.SetActive(false);
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            PlaceHead();
        }
    }

    private void PlaceHead()
    {
        if (headsPlaced >= slotPoints.Length)
            return;

        // วางหัว
        Instantiate(headPrefabs[headsPlaced],
                    slotPoints[headsPlaced].position,
                    Quaternion.identity);

        // ลบหัวออกจาก inventory
        InventoryManager.instance.RemoveItem("DavisHead" + (headsPlaced + 1));

        headsPlaced++;

        if (headsPlaced == slotPoints.Length)
        {
            if (rewardImage != null)
                rewardImage.SetActive(true);

            Debug.Log("เควสเสร็จสมบูรณ์!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
