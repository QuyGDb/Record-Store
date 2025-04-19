using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static class StaticEventHandler
{
    public static event Action<GameObject> OnXRGrabInteractableSelected;

    public static void InvokeXRGrabInteractableSelected(GameObject interactable)
    {
        OnXRGrabInteractableSelected?.Invoke(interactable);
    }
    public static event Action<GameObject> OnUIInteractableSelected;

    public static void InvokeUIInteractableSelected(GameObject interactable)
    {
        OnUIInteractableSelected?.Invoke(interactable);
    }
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

    public static event Action<ARCloudAnchor, AnchorType> OnInstantiateAtWall;

    public static void InvokeInstantiateAtWall(ARCloudAnchor aRAnchor, AnchorType anchorType)
    {
        OnInstantiateAtWall?.Invoke(aRAnchor, anchorType);
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

    public static event Action OnMovePortal;

    public static void InvokeMovePortal()
    {
        OnMovePortal?.Invoke();
    }

    public static event Action<string> OnNameMapText;

    public static void InvokeNameMapText(string es3name)
    {
        OnNameMapText?.Invoke(es3name);
    }
}
