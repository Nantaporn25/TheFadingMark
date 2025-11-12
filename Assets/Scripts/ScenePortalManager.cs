using UnityEngine;
using System.Collections;

public class ScenePortalManager : MonoBehaviour
{
    public static string nextSpawnPointName;

    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    IEnumerator SpawnPlayer()
    {
        yield return null; // รอ Scene โหลดเสร็จ

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player ไม่พบใน Scene");
            yield break;
        }

        // หา SpawnPoint ใน Scene
        SpawnPoint[] allPoints = FindObjectsOfType<SpawnPoint>();
        foreach (var sp in allPoints)
        {
            Debug.Log("พบ SpawnPoint: " + sp.spawnPointName);
            if (sp.spawnPointName == nextSpawnPointName)
            {
                player.transform.position = sp.transform.position;
                yield break;
            }
        }

        Debug.LogWarning("SpawnPoint ที่กำหนดไม่พบ, Player เกิดที่เดิม");
    }
}
