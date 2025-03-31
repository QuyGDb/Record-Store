using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(ScrollView))]
public class CloudAnchorsScrollView : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
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
        if (cloudAnchorDetails.Count == 0)
        {
            Debug.Log("No cloud anchor");
        }
        foreach (var cloudAnchor in cloudAnchorDetails)
        {
            GameObject gameObject = Instantiate(prefab, content.transform);
            cloudAnchorImages.Add(gameObject);
            gameObject.GetComponent<CloudAnchorImage>().anchorDetails = cloudAnchor.Value;
        }
    }

    private Sprite TextureToSprite(byte[] textureBytes)
    {
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (!texture.LoadImage(textureBytes))
        {
            Debug.LogError("Không thể load hình từ byte array.");
            return null;
        }

        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return newSprite;
    }

}
