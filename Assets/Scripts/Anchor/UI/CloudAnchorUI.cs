using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloudAnchorUI : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private bool isResolved = false;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (ApplicationManager.Instance.cloudAnchorsManager.currentAnchorDetails == null)
        {
            return;
        }
        image.sprite = HelperUtilities.SetSprite(ApplicationManager.Instance.cloudAnchorsManager.currentAnchorDetails.anchorImage);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isResolved)
        {
            ApplicationManager.Instance.cloudAnchorsManager.ResolveSelectedCloudAnchor();
            isResolved = true;
        }
        // Toggle alpha between 1 (visible) and 0 (invisible) on each click
        Color color = image.color;
        color.a = color.a == 1f ? 0f : 1f;
        image.color = color;
    }

}
