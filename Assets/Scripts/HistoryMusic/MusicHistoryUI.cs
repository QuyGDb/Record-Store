using UnityEngine;
using UnityEngine.Video;

public class MusicHistoryUI : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }
    public void PlayVideo(MusicGenreSO musicGenreSO)
    {

    }
}
