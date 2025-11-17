using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class SceneFader : MonoBehaviour
{
    public Image fadeImage;   // ใส่ FadeImage
    public float fadeSpeed = 1f;

    void Start()
    {
        // ตอนเข้า Scene ให้สว่างขึ้น
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float alpha = 1f;
        Color c = fadeImage.color;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float alpha = 0f;
        Color c = fadeImage.color;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene("FadeOne");
    }
}
