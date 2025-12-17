using UnityEngine;
using System.Collections.Generic;

public class NPCTrigger : MonoBehaviour
{
    public EnemyMovement enemyMovement;
    public Transform player;

    [Header("Dialogue Lines")]
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // แสดง Dialog
            DialogManager.Instance.StartDialog(dialogueLines);

            // เริ่มไล่ผู้เล่น
            if (enemyMovement != null)
            {
                enemyMovement.player = player;
                enemyMovement.StartChase();
            }
        }
    }
}
