using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class PauseVideoRawImage : MonoBehaviour, IPointerClickHandler
{
    VideoPlayer videoPlayer;
    private RawImage rawImage;
    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        rawImage = GetComponent<RawImage>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            rawImage.color = Color.gray;
        }
        else
        {
            videoPlayer.Play();
            rawImage.color = Color.white;
        }
    }
}
