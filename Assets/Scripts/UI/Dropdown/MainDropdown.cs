using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainDropdown : MonoBehaviour
{
    [SerializeField] Canvas[] canvas = new Canvas[3];
    TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }
    private void OnValueChanged(int arg0)
    {
        switch (arg0)
        {
            case 0:
                DeactivateAllCanvas();
                break;
            case 1:
                DeactivateAllCanvas();
                canvas[0].enabled = true;
                break;
            case 2:
                DeactivateAllCanvas();
                canvas[1].enabled = true;
                break;
            case 3:
                DeactivateAllCanvas();
                canvas[2].enabled = true;
                break;
        }
    }
    private void DeactivateAllCanvas()
    {
        foreach (var item in canvas)
        {
            item.enabled = false;
        }
    }

}
