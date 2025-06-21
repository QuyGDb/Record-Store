using System.Collections;
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

    [SerializeField] private string[] files;

    ES3Settings settings;

    string key = "cloudAnchorDetails";
    const string currentMap = "Current Map: ";
    [SerializeField] private Canvas mapCanvas;
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
            AnchorDetails anchorDetail = null;
            if (ES3.FileExists(fileName))
            {
                anchorDetail = ES3.Load<AnchorDetails>(key, fileName);
            }
            if (anchorDetail != null)
            {
                item.GetComponent<Image>().sprite = HelperUtilities.SetSprite(anchorDetail.anchorImage);
                item.GetComponentInChildren<TextMeshProUGUI>().text = fileName;
            }
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

    public void LoadBtn()
    {
        ApplicationManager.Instance.cloudAnchorsManager.ResolveSelectedCloudAnchor();
    }

    private IEnumerator ShowObjectsWithDelay()
    {
        yield return new WaitForSeconds(1f);

    }

    private void OnCloudAnchorResolved(bool success, string message)
    {
        if (success && mapCanvas != null)
        {
            mapCanvas.gameObject.SetActive(false);
            GameResources.Instance.instrumentShowcaseManager.ShowObjects();
            GameResources.Instance.pictureFrameManager.ShowObjects();
            // StartCoroutine(ShowObjectsWithDelay());
        }
    }

    private void OnEnable()
    {
        StaticEventHandler.OnCloudAnchorResolved += OnCloudAnchorResolved;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnCloudAnchorResolved -= OnCloudAnchorResolved;
    }
}
