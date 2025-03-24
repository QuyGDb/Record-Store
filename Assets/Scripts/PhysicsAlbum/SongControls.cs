using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SongControls : MonoBehaviour
{
    private Button[] buttons;
    [SerializeField] private AlbumSO albumSOs;
    private AudioSource audioSource;
    int currentTrack = 0;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }
    private void Start()
    {
        PlayTrack();

    }
    List<AudioClip> playlist;


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
        if (currentTrack < playlist.Count - 1)
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
        audioSource.clip = playlist[currentTrack];
        audioSource.Play();
    }
}
