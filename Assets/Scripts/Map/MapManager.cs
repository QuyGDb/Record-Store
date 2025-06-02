using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : MonoBehaviour
{
    public GameObject fileItemPrefab;
    public Transform contentPanel;
    [SerializeField] private TextMeshProUGUI currentMapText;
    private Button backBtn;
    private Canvas homeCanvas;

    [SerializeField] private string[] files;

    ES3Settings settings;

    Dictionary<string, AnchorDetails> anchorDetails = new Dictionary<string, AnchorDetails>();

    string key = "cloudAnchorDetails";
    const string currentMap = "Current Map: ";
    private void Start()
    {
        ShowFiles();
    }
    async void ShowFiles()
    {
        await SupabaseStorage.Instance.ListAndDownloadAllFiles();
        string path = Application.persistentDataPath;
        files = Directory.GetFiles(path, "*.es3");

        foreach (string filePath in files)
        {
            GameObject item = Instantiate(fileItemPrefab, contentPanel);
            string fileName = Path.GetFileName(filePath);
            settings = new ES3Settings(fileName);
            ES3.LoadInto(key, anchorDetails, settings);
            Sprite sprite = HelperUtilities.SetSprite(anchorDetails[key].anchorImage);
            item.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            item.GetComponent<Image>().sprite = sprite;
            Button btn = item.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnFileClicked(fileName));
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
