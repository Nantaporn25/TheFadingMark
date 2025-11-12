using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // คงอยู่ข้าม Scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveToScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return null; // รอ frame
        SceneManager.LoadScene(sceneName);
    }
}
