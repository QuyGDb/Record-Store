using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentShowcaseScrollView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject prefab;
    public InstrumentShowcaseListSO instrumentListSO;

    private void Awake()
    {
        StaticEventHandler.OnInstrumentListSOChanged += OnIntrumentShowcaseListSOChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstrumentListSOChanged -= OnIntrumentShowcaseListSOChanged;
    }
    private void Start()
    {
        if (instrumentListSO != null)
        {
            UpdateIntrumentScrollView();
        }
    }
    private void OnIntrumentShowcaseListSOChanged(InstrumentShowcaseListSO instrumentListSO)
    {
        this.instrumentListSO = instrumentListSO;
        UpdateIntrumentScrollView();

    }

    private void UpdateIntrumentScrollView()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var instrumentShowcase in instrumentListSO.instrumentShowcaseList)
        {
            GameObject GO = Instantiate(prefab, content);
            InstrumentImage instrumentImage = GO.GetComponent<InstrumentImage>();
            instrumentImage.image.sprite = instrumentShowcase.instrumentSprite;
            instrumentImage.instrumentShowcase = instrumentShowcase;
        }
    }
}
