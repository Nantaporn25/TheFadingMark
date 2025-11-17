using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // สำหรับ Editor เท่านั้น
#endif

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("PhoneScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // หยุด Play Mode ใน Editor
#else
        Application.Quit(); // ปิดเกมจริง
#endif
    }
}
