using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloudAnchorImage : MonoBehaviour, IPointerClickHandler
{
    public AnchorDetails anchorDetails;
    public Image targetImage;
    public TextMeshProUGUI[] textMeshProUGUIs;
    private Toggle toggle;
    private RectTransform contentTransform;
    public void OnPointerClick(PointerEventData eventData)
    {
        toggle.gameObject.SetActive(!toggle.gameObject.activeSelf);
        textMeshProUGUIs[0].gameObject.SetActive(!textMeshProUGUIs[0].gameObject.activeSelf);
        textMeshProUGUIs[1].gameObject.SetActive(!textMeshProUGUIs[1].gameObject.activeSelf);
    }

    private void Awake()
    {
        targetImage = GetComponent<Image>();
        textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
        toggle = GetComponentInChildren<Toggle>();
        contentTransform = transform.parent.GetComponent<RectTransform>();
        toggle.onValueChanged.AddListener(OnToggleChange);

    }
    private void Start()
    {
        SetSprite();
        SetInforAnchorImage();
    }
    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleChange);
    }
    void OnToggleChange(bool isOn)
    {
        StaticEventHandler.InvokeSelectCloudAnchor(isOn, anchorDetails.cloudAnchorId);
    }


    void SetInforAnchorImage()
    {
        textMeshProUGUIs[0].text = anchorDetails.anchorName;
        textMeshProUGUIs[1].text = anchorDetails.anchorDescription;
    }
    private Sprite SetSprite()
    {
        byte[] spriteBytes = anchorDetails.anchorImage;
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(spriteBytes);
        Sprite newSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        targetImage.sprite = newSprite;

        return newSprite;

    }


}
