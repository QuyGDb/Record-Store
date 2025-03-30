using System;
using System.Collections.Generic;
using System.Diagnostics;
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
}
