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
    public static event Action OnHostCurrentSelectAnchor;

    public static void InvokeHostCurrentSelectAnchor()
    {
        OnHostCurrentSelectAnchor?.Invoke();
    }


    public static event Action<ARCloudAnchor> OnInstantiateAtAnchor;

    public static void InvokeInstantiateAtAnchor(ARCloudAnchor aRAnchor)
    {
        OnInstantiateAtAnchor?.Invoke(aRAnchor);
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
    public static event Action<bool, string> OnCloudAnchorResolved;

    public static void InvokeCloudAnchorResolved(bool success, string message)
    {
        OnCloudAnchorResolved?.Invoke(success, message);
    }
}
