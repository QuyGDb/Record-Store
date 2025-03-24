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


}
