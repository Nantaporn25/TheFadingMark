using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed = 2f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float[] waitTimes; // ⭐ เวลารอแต่ละจุด

    private Rigidbody2D rb;
    private Animator anim;

    private int currentPointIndex = 0;
    private Vector2 moveDirection;
    private Vector2 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // กันพลาด
        if (waitTimes.Length != patrolPoints.Length)
        {
            Debug.LogError("waitTimes ต้องมีจำนวนเท่ากับ patrolPoints");
            return;
        }

        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Transform targetPoint = patrolPoints[currentPointIndex];

            // เดินไปจุด
            while (Vector2.Distance(transform.position, targetPoint.position) > 0.05f)
            {
                moveDirection = (targetPoint.position - transform.position).normalized;
                lastDirection = moveDirection;

                rb.linearVelocity = moveDirection * moveSpeed;
                UpdateAnimation(true);

                yield return null;
            }

            // ถึงจุดแล้ว
            rb.linearVelocity = Vector2.zero;
            UpdateAnimation(false);

            // ⭐ รอตามเวลาของจุดนี้
            yield return new WaitForSeconds(waitTimes[currentPointIndex]);

            // ไปจุดถัดไป
            currentPointIndex++;
            if (currentPointIndex >= patrolPoints.Length)
                currentPointIndex = 0;
        }
    }

    void UpdateAnimation(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);

        Vector2 dir = isWalking ? moveDirection : lastDirection;
        anim.SetFloat("InputX", dir.x);
        anim.SetFloat("InputY", dir.y);
    }
}

