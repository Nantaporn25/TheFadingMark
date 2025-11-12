using UnityEngine;

public class ScenePortal : MonoBehaviour
{
    [Header("Scene ที่จะโหลด")]
    public string sceneToLoad;

    [Header("ชื่อ SpawnPoint ใน Scene ใหม่")]
    public string spawnPointName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScenePortalManager.nextSpawnPointName = spawnPointName; // กำหนด SpawnPoint ของ Scene ใหม่
            GameManager.Instance.MoveToScene(sceneToLoad);
        }
    }
}
