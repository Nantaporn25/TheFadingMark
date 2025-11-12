using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 3f;
    public float maxZoom = 10f;
    public float zoomTarget = 5f;

    private Camera cam;
    private Bounds roomBounds;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam.orthographic)
            cam.orthographicSize = zoomTarget;

        DontDestroyOnLoad(gameObject);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (roomBounds.size != Vector3.zero)
        {
            float camHalfHeight = cam.orthographicSize;
            float camHalfWidth = cam.aspect * camHalfHeight;

            float minX = roomBounds.min.x + camHalfWidth;
            float maxX = roomBounds.max.x - camHalfWidth;
            float minY = roomBounds.min.y + camHalfHeight;
            float maxY = roomBounds.max.y - camHalfHeight;

            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomTarget, Time.deltaTime * zoomSpeed);
    }

    public void SetTarget(Transform newTarget) => target = newTarget;

    public void SetRoomBounds(Bounds bounds) => roomBounds = bounds;

    public void SetZoom(float newZoom) => zoomTarget = Mathf.Clamp(newZoom, minZoom, maxZoom);
}
