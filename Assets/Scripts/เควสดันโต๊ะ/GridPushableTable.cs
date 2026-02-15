using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GridPushableTable : MonoBehaviour
{
    public Transform[] targetPositions;          // จุดวางหลายตำแหน่ง
    public float moveSpeed = 3f;
    public LayerMask obstacleLayer;
    public Vector2 checkSizeMultiplier = new Vector2(0.9f, 0.9f);

    [HideInInspector]
    public bool isAtTarget = false;             // บอกว่าโต๊ะวางถูกตำแหน่ง

    private bool isMoving = false;
    private Vector3 targetPos;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip pushSound;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = targetPos;
                isMoving = false;
                CheckTargetPosition();
            }
        }
    }

    public void Push(Transform playerTransform)
    {
        if (isMoving) return;

        Vector3 direction = (transform.position - playerTransform.position).normalized;
        Vector3 moveDir = Vector3.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            moveDir = new Vector3(Mathf.Sign(direction.x), 0, 0);
        else
            moveDir = new Vector3(0, Mathf.Sign(direction.y), 0);

        Vector2 checkSize = new Vector2(
            GetComponent<Collider2D>().bounds.size.x * checkSizeMultiplier.x,
            GetComponent<Collider2D>().bounds.size.y * checkSizeMultiplier.y
        );

        if (!Physics2D.OverlapBox(transform.position + moveDir, checkSize, 0, obstacleLayer))
        {
            targetPos = transform.position + moveDir;
            isMoving = true;

            // 🔊 เล่นเสียงทุกครั้งที่เริ่มลากสำเร็จ
            if (audioSource != null && pushSound != null)
            {
                audioSource.PlayOneShot(pushSound);
            }
        }

    }

    private void CheckTargetPosition()
    {
        isAtTarget = false;
        foreach (var target in targetPositions)
        {
            if (target == null) continue;

            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                isAtTarget = true;
                break;
            }
        }
    }

    // --------- Gizmos ---------
    private void OnDrawGizmosSelected()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.green;
        Vector2 size = new Vector2(col.bounds.size.x * checkSizeMultiplier.x, col.bounds.size.y * checkSizeMultiplier.y);
        Gizmos.DrawWireCube(transform.position, size);

        if (targetPositions != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var target in targetPositions)
            {
                if (target != null)
                    Gizmos.DrawWireCube(target.position, size);
            }
        }
    }
}
