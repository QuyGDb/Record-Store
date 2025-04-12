using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class SongControls : MonoBehaviour
{
    private Button[] buttons;
    private AudioSource audioSource;
    private AlbumSO albumSO;
    private int currentTrack;
    private PhysicalCDAlbum physicalCDAlbum;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
        CDAlbumManager cdAlbumManager = GetComponentInParent<CDAlbumManager>();
        physicalCDAlbum = cdAlbumManager.albumSO.physicalCDAlbum;
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
        if (physicalCDAlbum == album.physicalCDAlbum)
        {
            albumSO = album;
            currentTrack = 0;
            PlayTrack(null);
        }
    }

    private UnityAction actionPlayPrevious;
    private UnityAction actionPause;
    private UnityAction actionPlayNext;

    void Start()
    {
        actionPlayPrevious = () => PlayPreviousTrack(buttons[0]);
        actionPause = () => PauseTrack(buttons[1]);
        actionPlayNext = () => PlayNextTrack(buttons[2]);

        buttons[0].onClick.AddListener(actionPlayPrevious);
        buttons[1].onClick.AddListener(actionPause);
        buttons[2].onClick.AddListener(actionPlayNext);
    }

    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(actionPlayPrevious);
        buttons[1].onClick.RemoveListener(actionPause);
        buttons[2].onClick.RemoveListener(actionPlayNext);
    }

    void PlayPreviousTrack(Button button)
    {
        if (currentTrack > 0)
        {
            currentTrack--;
            PlayTrack(button);
        }
    }

    void PlayNextTrack(Button button)
    {
        if (currentTrack < albumSO.songs.Count - 1)
        {
            currentTrack++;
            PlayTrack(button);
        }
    }

    void PauseTrack(Button button)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            button.GetComponent<SongControlsButton>().ClickEffect();
        }
        else
        {
            button.GetComponent<SongControlsButton>().ClickEffect();
            audioSource.Play();
        }
    }

    void PlayTrack(Button button)
    {
        if (physicalCDAlbum != albumSO.physicalCDAlbum)
            return;
        if (button != null)
            button.GetComponent<SongControlsButton>().ClickEffect();
        audioSource.clip = albumSO.songs[currentTrack].songClip;
        audioSource.Play();

        StaticEventHandler.InvokeSongChanged(albumSO, currentTrack);
        StopAllCoroutines();
        StartCoroutine(CheckTrackEndCoroutine());
    }

    private IEnumerator CheckTrackEndCoroutine()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying && audioSource.time > 0f);

        if (currentTrack < albumSO.songs.Count - 1)
        {
            currentTrack++;
            PlayTrack(null);
        }
    }
}
