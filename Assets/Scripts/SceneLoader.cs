using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private string currentMap;
    private GameObject player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.FindWithTag("Player");
        if (player != null) DontDestroyOnLoad(player);
    }

    public void LoadMap(string mapName, string spawnPointName)
    {
        if (!string.IsNullOrEmpty(currentMap))
        {
            SceneManager.UnloadSceneAsync(currentMap);
        }

        SceneManager.LoadSceneAsync(mapName, LoadSceneMode.Additive).completed += (op) =>
        {
            if (player == null)
                player = GameObject.FindWithTag("Player");

            // หา SpawnPoint ด้วยชื่อ
            GameObject spawnPoint = GameObject.Find(spawnPointName);
            if (spawnPoint != null && player != null)
            {
                player.transform.position = spawnPoint.transform.position;
            }
        };

        currentMap = mapName;
    }
}
