using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachObjectCanvas : MonoBehaviour
{
    private Button[] buttons;
    private ScrollRect scrollRect;
    private TMP_Dropdown musicObjectsDropdown;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        scrollRect = GetComponentInChildren<ScrollRect>();
        musicObjectsDropdown = GetComponentInChildren<TMP_Dropdown>();
        StaticEventHandler.OnAttachObjectManagerChanged += OnAttachObjectManagerChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnAttachObjectManagerChanged -= OnAttachObjectManagerChanged;
    }
    private void Start()
    {
        scrollRect.gameObject.SetActive(false);
        musicObjectsDropdown.gameObject.SetActive(false);
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
        if (scrollRect.gameObject.activeSelf)
        {
            musicObjectsDropdown.gameObject.SetActive(false);
            scrollRect.gameObject.SetActive(false);
        }
        else
        {
            musicObjectsDropdown.gameObject.SetActive(true);
            scrollRect.gameObject.SetActive(true);
        }
    }
}
