using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostAndResolveAnchorCanvas : MonoBehaviour
{
    private Button[] buttons;
    private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI cloudAnchorText;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        scrollRect = GetComponentInChildren<ScrollRect>();
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;
    }
    private void Start()
    {
        GameResources.Instance.cloudAnchorSceneText = cloudAnchorText;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
    }

    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        buttons[0].onClick.AddListener(manager.ResolveSelectedCloudAnchor);
        buttons[1].onClick.AddListener(manager.RemoveCloudAnchorInAnchorDetails);
    }


}
