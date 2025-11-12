using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public float interactionRange = 1.5f; // ระยะใกล้วัตถุ

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // ตรวจสอบปุ่ม E สำหรับเก็บของ
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable nearby = GetNearestInteractable();
            if (nearby != null)
                nearby.Interact();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // หา interactable ใกล้ที่สุด
    Interactable GetNearestInteractable()
    {
        Interactable[] all = Object.FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        Interactable nearest = null;
        float minDist = float.MaxValue;

        foreach (var i in all)
        {
            float dist = Vector2.Distance(transform.position, i.transform.position);
            if (dist < interactionRange && dist < minDist)
            {
                minDist = dist;
                nearest = i;
            }
        }
        return nearest;
    }
}
