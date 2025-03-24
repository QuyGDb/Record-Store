using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SongControls : MonoBehaviour
{
    private Button[] buttons;
    private AudioSource audioSource;
    private AlbumSO albumSO;
    private int currentTrack;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }
    private void OnEnable()
    {
        StaticEventHandler.OnStartFirstSong += HandleStartFirstSong;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnStartFirstSong -= HandleStartFirstSong;
    }
    private void HandleStartFirstSong(AlbumSO album)
    {
        if (album.physicalCDAlbum == PhysicalCDAlbum.Gieo)
        {
            albumSO = album;
            currentTrack = 0;
            PlayTrack();
        }
        if (album.physicalCDAlbum == PhysicalCDAlbum.SDDBP)
        {
            albumSO = album;
            currentTrack = 0;
            PlayTrack();
        }
    }

    private void Start()
    {

        buttons[0].onClick.AddListener(PlayPreviousTrack);
        buttons[1].onClick.AddListener(PauseTrack);
        buttons[2].onClick.AddListener(PlayNextTrack);
    }


    void PlayPreviousTrack()
    {
        if (currentTrack > 0)
        {
            currentTrack--;
            PlayTrack();
        }
    }

    void PlayNextTrack()
    {
        if (currentTrack < albumSO.songs.Count - 1)
        {
            currentTrack++;
            PlayTrack();
        }
    }

    void PauseTrack()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }

    void PlayTrack()
    {

        if (albumSO.albumName == "Gieo")
        {
            audioSource.clip = albumSO.songs[currentTrack].songClip;
            audioSource.Play();
            StaticEventHandler.InvokeSongChanged(albumSO, currentTrack);
        }
        if (albumSO.albumName == "SDDBP")
        {
            audioSource.clip = albumSO.songs[currentTrack].songClip;
            audioSource.Play();
            StaticEventHandler.InvokeSongChanged(albumSO, currentTrack);
        }
    }
}
