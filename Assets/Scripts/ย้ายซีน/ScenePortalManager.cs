using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenePortalManager : MonoBehaviour
{
    public static string nextSpawnPointName;

    void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    IEnumerator SpawnPlayer()
    {
        yield return null; // รอ Scene โหลด

        // หา Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player ไม่พบใน Scene!");
            yield break;
        }

        // หา CameraFollow แบบใหม่
        CameraFollow cam = Object.FindFirstObjectByType<CameraFollow>();

        // หา SpawnPoint แบบใหม่
        SpawnPoint[] allPoints = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        bool found = false;

        foreach (var sp in allPoints)
        {
            if (sp.spawnPointName == nextSpawnPointName)
            {
                player.transform.position = sp.transform.position;
                found = true;

                if (cam != null)
                    cam.SetTarget(player.transform);

                GameObject boundsObj = GameObject.Find("RoomBounds");
                if (boundsObj != null)
                {
                    BoxCollider2D box = boundsObj.GetComponent<BoxCollider2D>();
                    if (box != null)
                        cam.SetRoomBounds(box.bounds);
                }

                break;
            }
        }

        if (!found)
        {
            Debug.LogWarning("SpawnPoint ที่กำหนดไม่พบ, Player เกิดที่เดิม");
            if (cam != null)
                cam.SetTarget(player.transform);
        }
    }
}
