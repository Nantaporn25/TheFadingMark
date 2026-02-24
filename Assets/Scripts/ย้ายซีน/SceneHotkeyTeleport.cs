using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHotkeyTeleport : MonoBehaviour
{
    [Header("ลาก Scene Name ใส่ตามลำดับ 1-5")]
    public string[] sceneNames; // ใส่ 5 ช่องใน Inspector

    void Update()
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            // Alpha1 = ปุ่มเลขแถวบน
            // Keypad1 = เลข Numpad
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) ||
                Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                if (!string.IsNullOrEmpty(sceneNames[i]))
                {
                    SceneManager.LoadScene(sceneNames[i]);
                }
            }
        }
    }
}