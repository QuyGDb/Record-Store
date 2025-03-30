using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CloudAnchorsScrollView : MonoBehaviour
{
    private GameObject prefab;
    public GameObject content;
    private List<GameObject> cloudAnchorImages = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private void Awake()
    {
        StaticEventHandler.OnCloudAnchorDetailsChanged += OnCloudAnchorsManager;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnCloudAnchorDetailsChanged -= OnCloudAnchorsManager;
    }

    private void OnCloudAnchorsManager(Dictionary<string, AnchorDetails> cloudAnchorDetails)
    {
        textMeshProUGUI.text = $"Anchor đã lưu 1 {cloudAnchorDetails.Count}";
        if (cloudAnchorDetails == null)
            return;
        if (cloudAnchorDetails.Count == 0)
            return;
        foreach (var cloudAnchorImage in cloudAnchorImages)
        {
            Destroy(cloudAnchorImage);
        }
        if (cloudAnchorDetails == null || cloudAnchorDetails.Count == 0)
        {
            textMeshProUGUI.text = $"Anchor đã lưu 2 {cloudAnchorDetails.Count}";
            return;
        }
        foreach (var cloudAnchor in cloudAnchorDetails)
        {
            GameObject gameObject = Instantiate(prefab, content.transform);
            cloudAnchorImages.Add(gameObject);
            CloudAnchorImage cloudAnchorImage = gameObject.GetComponent<CloudAnchorImage>();
            cloudAnchorImage.image.sprite = cloudAnchor.Value.anchorSprite;
            cloudAnchorImage.textMeshProUGUIs[0].text = cloudAnchor.Value.anchorName;
            cloudAnchorImage.textMeshProUGUIs[1].text = cloudAnchor.Value.anchorDescription;

        }
    }

}
