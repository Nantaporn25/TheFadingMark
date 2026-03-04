using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;  // ✅ ต้องมีบรรทัดนี้

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Animator animator;

    [Header("Patrol Settings")]
    public List<Transform> waypoints = new List<Transform>();
    public List<float> waitTimes = new List<float>();
    private int currentWaypointIndex = 0;
    public bool loop = true;

    [Header("Chase Settings")]
    public Transform player;
    [HideInInspector] public Vector2 moveDirection;
    [HideInInspector] public bool isChasing = false;

    [Header("Light Settings")]
    public Light2D light2D; // 👈 เพิ่มตรงนี้ (ลากไฟฉายมาใส่ใน Inspector)

    private Vector2 smoothDirection;
    private bool waiting = false;

    private void Update()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else if (waypoints.Count > 0 && !waiting)
        {
            Patrol();
        }

        UpdateAnimation();
        UpdateLightRotation(); // 👈 เพิ่มตรงนี้
    }

    void Patrol()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position).normalized;
        moveDirection = direction;

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            float waitTime = waitTimes.Count > currentWaypointIndex ? waitTimes[currentWaypointIndex] : 1f;
            StartCoroutine(WaitAtWaypoint(waitTime));

            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
                currentWaypointIndex = loop ? 0 : waypoints.Count - 1;
        }
    }

    IEnumerator WaitAtWaypoint(float time)
    {
        waiting = true;
        moveDirection = Vector2.zero;
        yield return new WaitForSeconds(time);
        waiting = false;
    }

    void ChasePlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        moveDirection = direction;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void UpdateAnimation()
    {
        bool isMoving = moveDirection != Vector2.zero;
        animator.SetBool("isWalking", isMoving);

        smoothDirection = Vector2.Lerp(smoothDirection, moveDirection, 0.2f);

        if (isMoving)
        {
            animator.SetFloat("InputX", smoothDirection.x);
            animator.SetFloat("InputY", smoothDirection.y);
        }
    }

    // 👇 เพิ่มฟังก์ชันนี้เพื่อหมุนไฟฉายตามทิศทาง
    void UpdateLightRotation()
    {
        if (light2D == null) return;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            light2D.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void StartChase() => isChasing = true;
    public void StopChase() => isChasing = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameOverFadeTwo.Instance.GameOver();
        }
    }
}