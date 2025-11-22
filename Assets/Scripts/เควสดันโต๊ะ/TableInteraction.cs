using UnityEngine;

public class TableInteraction : MonoBehaviour
{
    private GridPushableTable table;

    void Start()
    {
        table = GetComponent<GridPushableTable>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            table.Push(other.transform);
        }
    }
}
