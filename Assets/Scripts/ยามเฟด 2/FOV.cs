using UnityEngine;
using System.Collections;

public class FOV : MonoBehaviour
{
    public float radius = 5f;
    [Range(1, 360)] public float angle = 45f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    public EnemyMovement enemyMovement;
    public bool CanSeePlayer { get; private set; }

    public int stepCount = 20; // จำนวนเส้น Grid ของ FOV

    private void Start()
    {
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            CheckFOV();
        }
    }

    private void CheckFOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0 && enemyMovement != null)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = ((Vector2)target.position - (Vector2)transform.position).normalized;

            Vector2 fovDirection = enemyMovement.moveDirection;
            if (fovDirection == Vector2.zero)
                fovDirection = Vector2.up;

            if (Vector2.Angle(fovDirection, directionToTarget) < angle / 2)
            {
                float distance = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distance, obstructionLayer))
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else
        {
            CanSeePlayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
#if UNITY_EDITOR
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        if (enemyMovement == null) return;
        Vector2 fovDirection = enemyMovement.moveDirection;
        if (fovDirection == Vector2.zero)
            fovDirection = Vector2.up;

        float stepAngle = angle / stepCount;

        // วาดเส้น FOV เป็น Fan
        for (int i = 0; i <= stepCount; i++)
        {
            float currentAngle = -angle / 2 + stepAngle * i;
            Vector2 lineDir = RotateVector(fovDirection, currentAngle);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + lineDir * radius);
        }

        // ถ้าเห็นผู้เล่น แสดงเส้นแดงตรงไปยังผู้เล่น
        if (CanSeePlayer && enemyMovement.player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, (Vector2)enemyMovement.player.position);
        }
#endif
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
