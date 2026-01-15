using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeSceneManager : MonoBehaviour
{
    public static FadeSceneManager instance;

    public Image fadeImage;
    public float fadeDuration = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 👈 สำคัญ: ทุกครั้งที่เริ่มฉากใหม่ ให้ Fade In
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int buildIndex)
    {
        StartCoroutine(FadeOutAndLoad(buildIndex));
    }

    IEnumerator FadeOutAndLoad(int buildIndex)
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(buildIndex);
        yield return null; // รอ 1 frame ให้ scene โหลด
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
