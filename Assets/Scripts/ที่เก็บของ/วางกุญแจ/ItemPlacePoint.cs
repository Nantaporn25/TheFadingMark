using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ItemPlacePoint : MonoBehaviour
{
    [Header("Thai item name required (must match exactly)")]
    public string requiredItemName;

    [Header("Scene to load after placing")]
    public string sceneToLoad;

    [Header("Delay before loading scene")]
    public float delayTime = 2f;

    [Header("Place sound")]
    public AudioClip placeSound;

    private bool playerInZone = false;
    private bool alreadyPlaced = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ถ้าไม่มี AudioSource ให้สร้างให้เลย
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerInZone && !alreadyPlaced && Input.GetKeyDown(KeyCode.E))
        {
            TryPlaceItem();
        }
    }

    void TryPlaceItem()
    {
        string selectedItemName = InventoryManager.instance.GetSelectedItemName();

        if (selectedItemName.Trim() == requiredItemName.Trim())
        {
            InventoryManager.instance.RemoveSelectedItem();
            alreadyPlaced = true;

            if (placeSound != null)
            {
                audioSource.PlayOneShot(placeSound);
            }

            StartCoroutine(LoadSceneWithDelay());
        }
    }

    IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;
    }
}