using System;
using UnityEngine;
using UnityEngine.UI;

public class AttachObjectCanvas : MonoBehaviour
{
    private Button[] buttons;
    private ScrollRect scrollRect;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        scrollRect = GetComponentInChildren<ScrollRect>();
        StaticEventHandler.OnAttachObjectManagerChanged += OnAttachObjectManagerChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnAttachObjectManagerChanged -= OnAttachObjectManagerChanged;
    }

    private void OnAttachObjectManagerChanged(AttachObjectManager manager)
    {
        buttons[0].onClick.AddListener((ToggleScrollRect));
        buttons[1].onClick.AddListener(() =>
        {
            ToggleScrollRect();
            manager.PlaceObject();
        });
        buttons[2].onClick.AddListener((manager.SaveObjectAtReleasePosition));
        buttons[3].onClick.AddListener((manager.DeteleCurrentSelectedObject));

    }

    void ToggleScrollRect()
    {
        scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
    }
}
