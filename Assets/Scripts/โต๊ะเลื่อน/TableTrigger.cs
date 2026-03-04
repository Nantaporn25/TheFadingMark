using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    [Header("Tables To Move")]
    public MovingTable[] tables;   // ใส่ได้หลายตัวใน Inspector

    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;

            foreach (MovingTable table in tables)
            {
                table.StartMove();
            }
        }
    }
}