using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    [Header("Tables To Move")]
    public MovingTable[] tables;

    [Header("Sound")]
    public AudioClip slideSound;
    private AudioSource audioSource;

    private bool activated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;

            // เล่นเสียงแค่ครั้งเดียว
            if (slideSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(slideSound);
            }

            foreach (MovingTable table in tables)
            {
                table.StartMove();
            }
        }
    }
}