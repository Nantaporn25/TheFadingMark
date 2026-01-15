using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class EnemyMove : MonoBehaviour
{
    [Header("Speed")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float[] waitTimes;

    [Header("Flashlight")]
    public Light2D flashlight;

    private Rigidbody2D rb;
    private Animator anim;
    private EnemyFOVFlashlight fov;
    private Transform player;

    private int currentPointIndex;
    private Vector2 moveDirection;
    private Vector2 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fov = GetComponent<EnemyFOVFlashlight>();

        if (!flashlight)
            flashlight = GetComponentInChildren<Light2D>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (patrolPoints.Length == 0 || waitTimes.Length != patrolPoints.Length)
        {
            Debug.LogError("PatrolPoints และ WaitTimes ต้องมีจำนวนเท่ากัน");
            return;
        }

        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        RotateFlashlight();
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // 🔴 CHASE MODE
            if (fov != null && fov.canSeePlayer && player != null)
            {
                ChasePlayer();
                yield return null;
                continue;
            }

            // 🟡 PATROL MODE
            Transform target = patrolPoints[currentPointIndex];

            while (Vector2.Distance(transform.position, target.position) > 0.05f)
            {
                if (fov != null && fov.canSeePlayer)
                    break;

                moveDirection = (target.position - transform.position).normalized;
                lastDirection = moveDirection;

                rb.linearVelocity = moveDirection * patrolSpeed;
                UpdateAnimation(true);

                yield return null;
            }

            rb.linearVelocity = Vector2.zero;
            UpdateAnimation(false);

            yield return new WaitForSeconds(waitTimes[currentPointIndex]);

            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        moveDirection = (player.position - transform.position).normalized;
        lastDirection = moveDirection;

        rb.linearVelocity = moveDirection * chaseSpeed;
        UpdateAnimation(true);
    }

    void UpdateAnimation(bool isWalking)
    {
        anim.SetBool("isWalking", isWalking);

        Vector2 dir = isWalking ? moveDirection : lastDirection;
        anim.SetFloat("InputX", dir.x);
        anim.SetFloat("InputY", dir.y);
    }

    void RotateFlashlight()
    {
        if (!flashlight) return;

        Vector2 dir = new Vector2(
            anim.GetFloat("InputX"),
            anim.GetFloat("InputY")
        );

        if (dir == Vector2.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        flashlight.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    // 💀 GAME OVER
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity = Vector2.zero;

            if (GameOverFadeManager.instance != null)
                GameOverFadeManager.instance.StartGameOver();
        }
    }
}
