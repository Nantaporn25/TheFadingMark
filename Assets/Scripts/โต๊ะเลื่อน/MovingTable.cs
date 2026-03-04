using UnityEngine;

public class MovingTable : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 3f;

    private bool move = false;

    void Update()
    {
        if (!move) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            move = false;   // หยุดถาวร
        }
    }

    public void StartMove()
    {
        move = true;
    }
}