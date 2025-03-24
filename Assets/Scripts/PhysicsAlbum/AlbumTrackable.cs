using DG.Tweening;
using UnityEngine;

public class AlbumTrackable : MonoBehaviour
{
    [SerializeField] private RectTransform albumInfo;
    [SerializeField] private RectTransform songs;
    [SerializeField] private RectTransform songImage;
    [SerializeField] private RectTransform songControls;
    [SerializeField] private Material songImageMAT;
    private void Awake()
    {
        albumInfo.gameObject.SetActive(false);
        songs.gameObject.SetActive(false);
        songControls.gameObject.SetActive(false);
        //songs.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        albumInfo.localScale = Vector3.zero;
        songImageMAT.SetFloat("_FadeAmount", 1f);
    }

    private void Start()
    {
        ShowAlbumInfo();
    }
    public void ShowAlbumInfo()
    {
        albumInfo.gameObject.SetActive(true);
        albumInfo.DOScale(new Vector3(1f, 1f, 1f), 2f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            songs.gameObject.SetActive(true);
            songImageMAT.DOFloat(0f, "_FadeAmount", 2f).OnComplete(() =>
            {
                songImage.GetComponent<Transform>()
                .DORotate(new Vector3(0f, 360f, 0f), 10f, RotateMode.FastBeyond360)
                 .SetLoops(-1, LoopType.Restart)
                  .SetEase(Ease.Linear);

            });
            songControls.gameObject.SetActive(true);

        });
    }


}
