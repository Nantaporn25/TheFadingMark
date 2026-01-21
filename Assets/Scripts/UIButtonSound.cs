using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public static UIButtonSound instance;
    public AudioSource clickAudio;

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

    public void PlayClick()
    {
        if (clickAudio != null)
            clickAudio.Play();
    }
}
