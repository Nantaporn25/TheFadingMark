using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFootstepAudio : MonoBehaviour
{
    [Header("References")]
    public PlayerController2D player;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    [Header("Footstep Settings")]
    public float moveThreshold = 0.1f;

    [Range(0f, 1f)]
    public float footstepVolume = 0.4f;

    public bool useFade = true;
    public float fadeSpeed = 5f;

    private Coroutine fadeRoutine;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = footstepVolume;
    }

    void Update()
    {
        // ถ้าถูกล็อกการเดิน → เงียบ
        if (player != null && !player.canMove)
        {
            StopFootstep();
            return;
        }

        bool isMoving = rb.linearVelocity.magnitude > moveThreshold;

        if (isMoving)
            PlayFootstep();
        else
            StopFootstep();
    }

    void PlayFootstep()
    {
        if (audioSource.isPlaying) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        audioSource.volume = footstepVolume;
        audioSource.Play();
    }

    void StopFootstep()
    {
        if (!audioSource.isPlaying) return;

        if (useFade)
            fadeRoutine = StartCoroutine(FadeOut());
        else
            audioSource.Stop();
    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = footstepVolume;
    }
}
