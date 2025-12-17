using UnityEngine;
using TMPro;

public class AfterSkill : MonoBehaviour
{
    public TextMeshProUGUI dialogTextUI;
    public GameObject dialogPanel;
    public string[] dialogs;

    public void ShowDialog(int index)
    {
        if (index < 0 || index >= dialogs.Length) return;

        dialogPanel.SetActive(true);
        dialogTextUI.text = dialogs[index];
    }

    public void CloseDialog()
    {
        dialogPanel.SetActive(false);
    }
}
