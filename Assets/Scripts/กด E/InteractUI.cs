using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public GameObject textUI;
    private bool playerInRange = false;
    private bool isShowing = false;

    void Update()
    {
        if (playerInRange)
        {
            if (!isShowing && Input.GetKeyDown(KeyCode.E))
            {
                // กด E เพื่อเปิด
                isShowing = true;
                textUI.SetActive(true);
            }
            else if (isShowing && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)))
            {
                // กด E หรือ Enter เพื่อปิด
                isShowing = false;
                textUI.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            textUI.SetActive(false);
            isShowing = false;
        }
    }
}
