using System;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloudAnchorUI : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public AnchorDetails anchorDetails;
    [HideInInspector] public Image targetImage;
    private bool isSelected = false;
    private Color defaultColor;
    private Color selectedColor;
    public bool isResolved = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isResolved) return;
        isSelected = !isSelected;
        if (isSelected)
        {
            targetImage.color = selectedColor;
        }
        else
        {
            targetImage.color = defaultColor;
        }
        StaticEventHandler.InvokeSelectCloudAnchor(isSelected, anchorDetails.cloudAnchorId);
    }

    private void Awake()
    {
        defaultColor = Color.white;
        selectedColor = new Color(0.5f, 0.5f, 0.5f, 1);
        targetImage = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        SetSprite();
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
