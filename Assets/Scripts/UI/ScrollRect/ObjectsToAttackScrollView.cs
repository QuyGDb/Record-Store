using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsToAttackScrollView : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject prefab;
    public List<InstrumentDetails> musicObjects = new List<InstrumentDetails>();

    private void Awake()
    {
        StaticEventHandler.OnObjectMusicListChanged += OnObjectMusicListChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnObjectMusicListChanged -= OnObjectMusicListChanged;
    }
    private void Start()
    {
        if (musicObjects != null)
        {
            UpdateScrollView();
        }
    }
    private void OnObjectMusicListChanged(MusicObjectListSO sO)
    {
        musicObjects = sO.musicObjects;
        UpdateScrollView();

    }

    private void UpdateScrollView()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var musicObject in musicObjects)
        {
            GameObject GO = Instantiate(prefab, content);
            InstrumentImage instrumentImage = GO.GetComponent<InstrumentImage>();
            instrumentImage.image.sprite = musicObject.instrumentSprite;
            instrumentImage.instrmentDetails = musicObject;
        }
    }
}
