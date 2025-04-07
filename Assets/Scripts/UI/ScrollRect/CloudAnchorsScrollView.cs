using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CloudAnchorsScrollView : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public GameObject content;
    private List<GameObject> cloudAnchorImages = new List<GameObject>();
    private Dictionary<string, AnchorDetails> cloudAnchorDetails = new Dictionary<string, AnchorDetails>();
    private Image scrollViewImage;

    private void Start()
    {
        ES3.Load("cloudAnchorDetails", cloudAnchorDetails);
        StaticEventHandler.InvokeAnchorDetailsChanged(cloudAnchorDetails);
    }
    private void Awake()
    {
        StaticEventHandler.OnAnchorDetailsChanged += OnAnchorDetailsChanged;
        scrollViewImage = GetComponent<Image>();
        GameResources.Instance.cloudAnchorListScrollViewImage = scrollViewImage;
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
        }
    }


}
