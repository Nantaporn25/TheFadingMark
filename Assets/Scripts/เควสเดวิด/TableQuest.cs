using UnityEngine;

public class TableQuest : MonoBehaviour
{
    [Header("ตำแหน่งที่หัวจะไปโผล่บนโต๊ะ")]
    public Transform[] slotPoints;   // 3 ตำแหน่ง

    [Header("Prefab หัวเดวิส (3 อัน)")]
    public GameObject[] headPrefabs; // 3 Prefab

    [Header("ภาพรางวัล (ปิดไว้ก่อน)")]
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
        // ถ้าวางครบแล้ว หยุด
        if (headsPlaced >= slotPoints.Length)
            return;

        // วาง prefab ตามลำดับ
        Instantiate(headPrefabs[headsPlaced],
                    slotPoints[headsPlaced].position,
                    Quaternion.identity);

        headsPlaced++;

        // ถ้าครบแล้ว → แสดงรางวัล
        if (headsPlaced == slotPoints.Length)
        {
            if (rewardImage != null)
                rewardImage.SetActive(true);

            Debug.Log("เควสเสร็จแล้ว!");
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
