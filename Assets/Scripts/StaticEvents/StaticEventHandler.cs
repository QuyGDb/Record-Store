using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static event Action<AnchorsManager> OnAnchorsManager;

    public static void InvokeAnchorsManager(AnchorsManager anchorsManager)
    {
        OnAnchorsManager?.Invoke(anchorsManager);
    }
    public static event Action<CloudAnchorsManager> OnCloudAnchorsManager;

    public static void InvokeCloudAnchorsManager(CloudAnchorsManager cloudAnchorsManager)
    {
        OnCloudAnchorsManager?.Invoke(cloudAnchorsManager);
    }
    public static event Action<string, string> OnSendAnchorInfo;

    public static void InvokeSendInfo(string name, string description)
    {
        OnSendAnchorInfo?.Invoke(name, description);
    }

    public static event Action<bool, string> OnSelectCloudAnchor;

    public static void InvokeSelectCloudAnchor(bool isSelect, string cloudAnchorId)
    {
        OnSelectCloudAnchor?.Invoke(isSelect, cloudAnchorId);
    }
    public static event Action<Dictionary<string, AnchorDetails>> OnCloudAnchorDetailsChanged;

    public static void InvokeCloudAnchorDetailsChanged(Dictionary<string, AnchorDetails> cloudAnchorDetails)
    {
        OnCloudAnchorDetailsChanged?.Invoke(cloudAnchorDetails);
    }
    public static event Action<MusicObjectListSO> OnObjectMusicListChanged;

    public static void InvokeObjectMusicListChanged(MusicObjectListSO objectMusicList)
    {
        OnObjectMusicListChanged?.Invoke(objectMusicList);
    }

    public static event Action<InstrumentDetails, bool> OnInstrumentSelected;

    public static void InvokeInstrumentSelected(InstrumentDetails instrument, bool isSelected)
    {
        OnInstrumentSelected?.Invoke(instrument, isSelected);
    }

    public static event Action<ARCloudAnchor> OnCurrentCloudAnchorChanged;

    public static void InvokeCurrentCloudAnchorChanged(ARCloudAnchor cloudAnchor)
    {
        OnCurrentCloudAnchorChanged?.Invoke(cloudAnchor);
    }

    public static event Action<AttachObjectManager> OnAttachObjectManagerChanged;

    public static void InvokeAttachObjectManagerChanged(AttachObjectManager attachObjectManager)
    {
        OnAttachObjectManagerChanged?.Invoke(attachObjectManager);
    }

    public static event Action<ARCloudAnchor> OnCloudAnchorSelected;

    public static void InvokeCloudAnchorSelected(ARCloudAnchor cloudAnchor)
    {
        OnCloudAnchorSelected?.Invoke(cloudAnchor);
    }
}
