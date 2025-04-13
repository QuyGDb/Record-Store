using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstrumentVideoHandler : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private Slider slider;
    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        slider.onValueChanged.AddListener((value) => { OnValueSliderChange(); });
    }
    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            slider.value = (float)videoPlayer.time;
        }

    }
    private void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener((value) => { OnValueSliderChange(); });
        }
    }
    private void OnValueSliderChange()
    {
        if (videoPlayer != null)
        {
            videoPlayer.time = slider.value;
        }
    }

    public void PlayVideo(VideoClip videoClip)
    {
        if (videoPlayer != null)
        {
            videoPlayer.clip = videoClip;
            slider.minValue = 0f;
            slider.maxValue = (float)videoClip.length;
            videoPlayer.Play();
        }
    }
}
