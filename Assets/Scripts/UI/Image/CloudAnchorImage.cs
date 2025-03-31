using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloudAnchorImage : MonoBehaviour, IPointerClickHandler
{
    public AnchorDetails anchorDetails;
    public Image image;
    public TextMeshProUGUI[] textMeshProUGUIs;
    private Toggle toggle;
    public void OnPointerClick(PointerEventData eventData)
    {
        toggle.gameObject.SetActive(!toggle.gameObject.activeSelf);
        textMeshProUGUIs[0].gameObject.SetActive(!textMeshProUGUIs[0].gameObject.activeSelf);
        textMeshProUGUIs[1].gameObject.SetActive(!textMeshProUGUIs[1].gameObject.activeSelf);
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
        toggle = GetComponentInChildren<Toggle>();
    }
    private void Start()
    {
        byte[] spriteBytes = anchorDetails.anchorImage;
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(spriteBytes);
        Sprite newSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        image.sprite = newSprite;
        textMeshProUGUIs[0].text = anchorDetails.anchorName;
        textMeshProUGUIs[1].text = anchorDetails.anchorDescription;
        toggle.onValueChanged.AddListener(OnTioggleChange);
    }
    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnTioggleChange);
    }
    void OnTioggleChange(bool isOn)
    {
        StaticEventHandler.InvokeSelectCloudAnchor(isOn, anchorDetails.cloudAnchorId);
    }
}
