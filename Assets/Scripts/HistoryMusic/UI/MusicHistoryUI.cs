using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.Video;

public class MusicHistoryUI : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private Slider slider;
    private Button button;
    private AudioSource audioSource;
    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        slider = GetComponentInChildren<Slider>();
        button = GetComponentInChildren<Button>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        button.onClick.AddListener(TurnOffThisUI);
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();
        audioSource.Play();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(TurnOffThisUI);
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }
    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            slider.value = (float)(videoPlayer.time / videoPlayer.clip.length);
        }
    }


    public void PlayVideo(VideoClip videoClip)
    {
        videoPlayer.clip = videoClip;
        videoPlayer.Play();
    }

    void TurnOffThisUI()
    {
        gameObject.SetActive(false);
    }

}
