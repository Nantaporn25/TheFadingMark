using UnityEngine;

public class MapTransition : MonoBehaviour
{
    public string nextMapName;     // Scene ปลายทาง
    public string spawnPointName;  // ชื่อ SpawnPoint ใน Scene ใหม่

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.Instance.LoadMap(nextMapName, spawnPointName);
        }
    }
}
