using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static class StaticEventHandler
{

    public static event Action<AlbumSO, int> OnSongChanged;

    public static void InvokeSongChanged(AlbumSO album, int index)
    {
        OnSongChanged?.Invoke(album, index);
    }
    public static event Action<AlbumSO> OnStartFirstSong;

    public static void InvokeStartFirstSong(AlbumSO album)
    {
        OnStartFirstSong?.Invoke(album);
    }

    public static event Action<string, AnchorType> OnSendAnchorInfo;

    public static void InvokeSendInfo(string name, AnchorType anchorType)
    {
        OnSendAnchorInfo?.Invoke(name, anchorType);
    }

    public static event Action<bool, string> OnSelectCloudAnchor;

    public static void InvokeSelectCloudAnchor(bool isSelect, string cloudAnchorId)
    {
        OnSelectCloudAnchor?.Invoke(isSelect, cloudAnchorId);
    }

    public static event Action<InstrumentShowcaseListSO> OnInstrumentListSOChanged;

    public static void InvokeInstrumentShowCaseListVNChanged(InstrumentShowcaseListSO objectMusicList)
    {
        OnInstrumentListSOChanged?.Invoke(objectMusicList);
    }

    public static event Action<InstrumentShowcaseSO, bool> OnInstrumentSOSelected;

    public static void InvokeInstrumentShowcaseSOSelected(InstrumentShowcaseSO instrument, bool isSelected)
    {
        OnInstrumentSOSelected?.Invoke(instrument, isSelected);
    }

    public static event Action<ARAnchor> OnCurrentAnchorChanged;

    public static void InvokeCurrentAnchorChanged(ARAnchor cloudAnchor)
    {
        OnCurrentAnchorChanged?.Invoke(cloudAnchor);
    }

    public static event Action<InstrumentsManager> OnInstrumentsManagerChanged;

    public static void InvokeInstrumentsManagerChanged(InstrumentsManager attachObjectManager)
    {
        OnInstrumentsManagerChanged?.Invoke(attachObjectManager);
    }

    public static event Action<ARAnchor> OnAnchorSelected;

    public static void InvokeAnchorSelected(ARAnchor cloudAnchor)
    {
        OnAnchorSelected?.Invoke(cloudAnchor);
    }

    public static event Action<int> OnMainDropdownChanged;

    public static void InvokeMainDropdownChanged(int index)
    {
        OnMainDropdownChanged?.Invoke(index);
    }

    public static event Action<InstrumentShowcase> OnInstrumentShowcaseChanged;
    public static void InvokeInstrumentShowcaseChanged(InstrumentShowcase instrumentShowcase)
    {
        OnInstrumentShowcaseChanged?.Invoke(instrumentShowcase);
    }

    public static event Action<Transform> OnRotateObjectChanged;

    public static void InvokeRotateObjectChanged(Transform transform)
    {
        OnRotateObjectChanged?.Invoke(transform);
    }

    public static event Action<ARCloudAnchor, AnchorType> OnInstantiateAtAnchor;

    public static void InvokeInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType anchorType)
    {
        OnInstantiateAtAnchor?.Invoke(aRAnchor, anchorType);
    }

    public static event Action<Anchor, bool> OnAnchorCreated;

    public static void InvokeAnchorChanged(Anchor anchor, bool isSelect)
    {
        OnAnchorCreated?.Invoke(anchor, isSelect);
    }
    public static event Action<ARAnchor> OnAnchorRemoved;

    public static void InvokeAnchorRemoved(ARAnchor anchor)
    {
        OnAnchorRemoved?.Invoke(anchor);
    }

    public static event Action<Dictionary<string, AnchorDetails>> OnAnchorDetailsChanged;

    public static void InvokeAnchorDetailsChanged(Dictionary<string, AnchorDetails> anchorDetails)
    {
        OnAnchorDetailsChanged?.Invoke(anchorDetails);
    }
}
