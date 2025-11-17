using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public Animator anim;
    public float moveSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public GameObject flashlight;
    private static bool flashlightOn = false;

    private float x;
    private float y;
    private Vector2 input;
    private bool moving;

    private bool canHide = false;
    private bool hiding = false;

    [HideInInspector] public bool canMove = true; // เพิ่มตรงนี้ เพื่อ DialogTrigger ใช้ได้

    public float interactionRange = 1.5f; // ระยะใกล้วัตถุ

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (flashlight != null)
            flashlight.SetActive(flashlightOn);
    }

    private void Update()
    {
        // หยุดเดินถ้า canMove = false
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("Moving", false);
            return;
        }

        // ระบบซ่อนตัว
        if (canHide && Input.GetKeyDown(KeyCode.E))
        {
            hiding = !hiding;
            sr.enabled = !hiding;

            if (hiding)
                flashlight.SetActive(false);
            else
                flashlight.SetActive(flashlightOn);
        }

        if (hiding)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("Moving", false);
            return;
        }

        GetInput();
        Animate();

        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightOn = !flashlightOn;
            flashlight.SetActive(flashlightOn);
        }

        if (flashlightOn && input != Vector2.zero)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            flashlight.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove || hiding)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = input * moveSpeed;
    }

    private void GetInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y).normalized;
    }

    private void Animate()
    {
        moving = input.magnitude > 0.1f;
        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }
        anim.SetBool("Moving", moving);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HideSpot"))
            canHide = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideSpot"))
            canHide = false;
    }
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