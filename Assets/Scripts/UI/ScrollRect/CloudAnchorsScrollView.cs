using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudAnchorsScrollView : MonoBehaviour
{
    private GameObject prefab;
    public GameObject content;
    private List<GameObject> cloudAnchorImages = new List<GameObject>();
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
        foreach (var cloudAnchorImage in cloudAnchorImages)
        {
            Destroy(cloudAnchorImage);
        }
        foreach (var cloudAnchor in cloudAnchorDetails)
        {
            GameObject gameObject = Instantiate(prefab, content.transform);
            cloudAnchorImages.Add(gameObject);
            CloudAnchorImage cloudAnchorImage = gameObject.GetComponent<CloudAnchorImage>();
            cloudAnchorImage.image.sprite = cloudAnchor.Value.anchorrSprite;
            cloudAnchorImage.textMeshProUGUIs[0].text = cloudAnchor.Value.anchorName;
            cloudAnchorImage.textMeshProUGUIs[1].text = cloudAnchor.Value.anchorDescription;

        }
    }

}
