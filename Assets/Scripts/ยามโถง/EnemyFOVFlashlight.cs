using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyFOVFlashlight : MonoBehaviour
{
    public Light2D flashlight;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    [HideInInspector]
    public bool canSeePlayer;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (!flashlight)
            flashlight = GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        canSeePlayer = CheckFlashlightVision();
    }

    bool CheckFlashlightVision()
    {
        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            flashlight.pointLightOuterRadius,
            playerLayer
        );

        if (!player) return false;

        Vector2 facingDir = GetFacingDirection();
        Vector2 dirToPlayer =
            (player.transform.position - transform.position).normalized;

        float angle = Vector2.Angle(facingDir, dirToPlayer);
        if (angle > flashlight.pointLightOuterAngle * 0.5f)
            return false;

        float distance = Vector2.Distance(
            transform.position,
            player.transform.position
        );

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dirToPlayer,
            distance,
            obstacleLayer
        );

        if (hit) return false;

        return true;
    }

    Vector2 GetFacingDirection()
    {
        Vector2 dir = new Vector2(
            anim.GetFloat("InputX"),
            anim.GetFloat("InputY")
        );

        if (dir == Vector2.zero)
            dir = Vector2.down;

        return dir.normalized;
    }

    // 🎨 Gizmos แสดงกรวยมองเห็น
    void OnDrawGizmos()
    {
        if (!flashlight) return;

        Vector3 origin = transform.position;
        float radius = flashlight.pointLightOuterRadius;
        float halfAngle = flashlight.pointLightOuterAngle * 0.5f;

        Vector2 facingDir = Application.isPlaying
            ? GetFacingDirection()
            : transform.up;

        Gizmos.color = canSeePlayer
            ? new Color(1f, 0f, 0f, 0.4f)
            : new Color(1f, 1f, 0f, 0.3f);

        Vector2 leftDir = Quaternion.Euler(0, 0, halfAngle) * facingDir;
        Vector2 rightDir = Quaternion.Euler(0, 0, -halfAngle) * facingDir;

        Gizmos.DrawLine(origin, origin + (Vector3)(leftDir * radius));
        Gizmos.DrawLine(origin, origin + (Vector3)(rightDir * radius));

        int segments = 20;
        Vector3 prev = origin + (Vector3)(rightDir * radius);

        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / segments;
            float ang = Mathf.Lerp(-halfAngle, halfAngle, t);
            Vector2 dir = Quaternion.Euler(0, 0, ang) * facingDir;
            Vector3 point = origin + (Vector3)(dir * radius);

            Gizmos.DrawLine(prev, point);
            prev = point;
        }

        Gizmos.DrawLine(origin, origin + (Vector3)(facingDir * radius));
    }
}
