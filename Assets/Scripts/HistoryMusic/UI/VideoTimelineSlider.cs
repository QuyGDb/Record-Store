using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoTimelineSlider : MonoBehaviour, IDragHandler
{
    private Slider slider;
    private VideoPlayer videoPlayer;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        videoPlayer = transform.parent.GetComponentInChildren<VideoPlayer>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        videoPlayer.time = videoPlayer.clip.length * slider.value;
    }


}
