using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverFadeTwo : MonoBehaviour
{
    public static GameOverFadeTwo Instance;

    [Header("UI")]
    public GameObject gameOverUI;
    public CanvasGroup fadeCanvas;

    //[Header("Audio")]
    //public AudioSource sceneBGM;

    [Header("Scene")]
    public string menuSceneName = " ";

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    private bool isGameOver = false;
    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameOverUI.SetActive(false);
        fadeCanvas.alpha = 0f;
    }

    private void Update()
    {
        if (isGameOver && !isFading && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeAndGoToMenu());
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverUI.SetActive(true);

        //if (sceneBGM != null)
        //    sceneBGM.Stop();

        Time.timeScale = 0f;
    }

    IEnumerator FadeAndGoToMenu()
    {
        isFading = true;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
