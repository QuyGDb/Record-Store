using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstrumentVideoHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private Slider slider;
    private AudioSource audioSource;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        slider = GetComponentInChildren<Slider>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();
        audioSource.Play();
    }

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            slider.value = (float)(videoPlayer.time / videoPlayer.clip.length);
        }

    }
    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    public void PlayVideo(VideoClip videoClip)
    {
        if (videoPlayer != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }
    }
}
