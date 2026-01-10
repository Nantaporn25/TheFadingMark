using UnityEngine;
using UnityEngine.UI;

public class PosterManager : MonoBehaviour
{
    public static PosterManager instance;

    [Header("UI")]
    public GameObject posterUI;
    public Image posterImageUI;
    public Image posterIcon;
    public Sprite defaultPoster;

    private Sprite posterSprite;
    private bool isShowing = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (posterUI != null) posterUI.SetActive(false);
            if (posterIcon != null) posterIcon.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🪧 เก็บโปสเตอร์ (แผ่นเดียว)
    public void PickupPoster(Sprite sprite)
    {
        posterSprite = sprite;

        if (posterIcon != null)
            posterIcon.enabled = true;

        Debug.Log("🪧 เก็บโปสเตอร์แล้ว");
    }

    void Update()
    {
        if (posterSprite == null) return;

        // 🔑 เปิด / ปิด ด้วย Z
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isShowing) ShowPoster();
            else ClosePoster();
        }
    }

    void ShowPoster()
    {
        if (posterUI != null && posterImageUI != null)
        {
            posterUI.SetActive(true);
            posterImageUI.sprite =
                posterSprite != null ? posterSprite : defaultPoster;

            isShowing = true;
            Time.timeScale = 0f;
        }
    }

    void ClosePoster()
    {
        if (posterUI != null)
            posterUI.SetActive(false);

        isShowing = false;
        Time.timeScale = 1f;
    }
}
