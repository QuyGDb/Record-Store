using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloudAnchorsScrollView : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public GameObject content;
    private List<GameObject> cloudAnchorImages = new List<GameObject>();
    private Dictionary<string, AnchorDetails> cloudAnchorDetails = new Dictionary<string, AnchorDetails>();

    private void Start()
    {
        cloudAnchorDetails = ES3.Load("cloudAnchorDetails", cloudAnchorDetails);
        StaticEventHandler.InvokeAnchorDetailsChanged(cloudAnchorDetails);
    }
    private void Awake()
    {
        StaticEventHandler.OnAnchorDetailsChanged += OnAnchorDetailsChanged;
        GameResources.Instance.contentCloudAnchor = content;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnAnchorDetailsChanged -= OnAnchorDetailsChanged;
    }

    private void OnAnchorDetailsChanged(Dictionary<string, AnchorDetails> cloudAnchorDetails)
    {
        foreach (var cloudAnchorImage in cloudAnchorImages)
        {
            Destroy(cloudAnchorImage);
        }
        foreach (var cloudAnchor in cloudAnchorDetails)
        {
            GameObject gameObject = Instantiate(prefab, content.transform);
            cloudAnchorImages.Add(gameObject);
            gameObject.GetComponent<CloudAnchorUI>().anchorDetails = cloudAnchor.Value;
            if (GameResources.Instance.resolveCloudAnchorIdList.Contains(cloudAnchor.Key))
            {
                CloudAnchorUI cloudAnchorUI = gameObject.GetComponent<CloudAnchorUI>();
                cloudAnchorUI.targetImage.color = Color.cyan;
                cloudAnchorUI.isResolved = true;
            }
        }
    }


}
