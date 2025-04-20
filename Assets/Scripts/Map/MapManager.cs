using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject fileItemPrefab;
    public Transform contentPanel;
    [SerializeField] private TextMeshProUGUI currentMapText;
    const string currentMap = "Current Map: ";
    private void Start()
    {
        ShowFiles();
    }
    async void ShowFiles()
    {
        await SupabaseStorage.Instance.ListAndDownloadAllFiles();
        string path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(path, "*.es3");

        foreach (string filePath in files)
        {

            GameObject item = Instantiate(fileItemPrefab, contentPanel);
            string fileName = Path.GetFileName(filePath);

            item.GetComponentInChildren<TextMeshProUGUI>().text = fileName;

            Button btn = item.GetComponent<Button>();
            if (btn != null)
            {
                string capturedFileName = fileName;
                btn.onClick.AddListener(() => OnFileClicked(capturedFileName));
            }
        }

    }

    void OnFileClicked(string fileName)
    {
        Settings.es3Name = fileName;
        currentMapText.text = currentMap + fileName;
        StaticEventHandler.InvokeNameMapText(fileName);
    }
}
