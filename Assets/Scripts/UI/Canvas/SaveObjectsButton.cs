using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveObjectsButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener((GameResources.Instance.transformObjectsManager.SaveTransformSelectInstrument));
    }
    private void Start()
    {
        button.onClick.AddListener((GameResources.Instance.transformObjectsManager.SaveTransformSelectInstrument));
    }

}






