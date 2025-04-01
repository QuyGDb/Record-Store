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
        StaticEventHandler.OnObjectMusicListChanged += UpdateScrollView;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnObjectMusicListChanged -= UpdateScrollView;
    }

    private void UpdateScrollView(MusicObjectListSO sO)
    {
        musicObjects = sO.musicObjects;
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        foreach (var musicObject in musicObjects)
        {
            GameObject GO = Instantiate(prefab, content);
            InstrumentImage instrumentImage = GO.GetComponent<InstrumentImage>();
            instrumentImage.instrumentImage.sprite = musicObject.instrumentSprite;
            instrumentImage.instrmentDetails = musicObject;
        }

    }
}
