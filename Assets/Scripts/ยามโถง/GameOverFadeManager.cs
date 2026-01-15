using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameOverFadeManager : MonoBehaviour
{
    public static GameOverFadeManager instance;

    [Header("UI")]
    public Image bgDarkPanel;      // พื้นดำหลังข้อความ
    public Image fadeBlackImage;   // เฟดเปลี่ยนฉาก
    public TextMeshProUGUI gameOverText;

    [Header("Scene")]
    public string restartSceneName;

    public float fadeSpeed = 1.5f;

    bool waitingInput = false;
    bool isFading = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetAlpha(fadeBlackImage, 0f);
        SetAlpha(gameOverText, 0f);

        if (bgDarkPanel)
            bgDarkPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (waitingInput && !isFading && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    public void StartGameOver()
    {
        Time.timeScale = 0f;

        if (bgDarkPanel)
            bgDarkPanel.gameObject.SetActive(true);

        SetAlpha(gameOverText, 1f);

        waitingInput = true;
    }

    IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        float alpha = fadeBlackImage.color.a;

        while (alpha < 1f)
        {
            alpha += Time.unscaledDeltaTime * fadeSpeed;
            SetAlpha(fadeBlackImage, alpha);
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(restartSceneName);
    }

    void SetAlpha(Graphic g, float a)
    {
        if (!g) return;

        Color c = g.color;
        c.a = a;
        g.color = c;
    }
}
