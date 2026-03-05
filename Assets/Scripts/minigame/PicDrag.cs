using UnityEngine;

public class PicDrag : MonoBehaviour
{
    public Transform slot;   // Slot ของภาพ
    public bool locked = false;

    [Header("Sound")]
    public AudioClip placeSound;
    private AudioSource audioSource;

    private Vector2 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        float distance = Vector2.Distance(transform.position, slot.position);

        if (distance <= 1f) // tolerance 1 หน่วย (ปรับได้)
        {
            transform.position = slot.position;
            locked = true;

            if (placeSound != null)
            {
                audioSource.PlayOneShot(placeSound);
            }

            Debug.Log($"{name} locked!");
        }
        else
        {
            transform.position = initialPosition;
            locked = false;
        }
    }
}