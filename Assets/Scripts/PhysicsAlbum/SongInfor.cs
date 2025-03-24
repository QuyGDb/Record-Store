using DG.Tweening;
using EasyTextEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongInfor : MonoBehaviour
{
    Image songImage;
    TextMeshProUGUI songName;
    TextEffect songNameEffect;
    private void Awake()
    {
        songImage = GetComponentInChildren<Image>();
        songName = GetComponentInChildren<TextMeshProUGUI>();
        songNameEffect = songName.GetComponent<TextEffect>();
    }
    private void OnEnable()
    {
        StaticEventHandler.OnSongChanged += UpdateUITransition;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnSongChanged -= UpdateUITransition;
    }

    private void UpdateUITransition(AlbumSO albumSO, int index)
    {
        if (albumSO.physicalCDAlbum == PhysicalCDAlbum.Gieo)
        {
            ApplyUITransition(albumSO.songs[index]);
        }
        if (albumSO.physicalCDAlbum == PhysicalCDAlbum.SDDBP)
        {
            ApplyUITransition(albumSO.songs[index]);
        }

    }

    private void ApplyUITransition(SongDescription song)
    {
        songNameEffect.gameObject.SetActive(false);
        songName.text = song.songName;
        songNameEffect.gameObject.SetActive(true);

        Material songImageMAT = transform.parent.GetComponent<CDAlbumDisplayController>().songImageMAT;
        songImageMAT.DOFloat(1f, "_FadeAmount", 1f).OnComplete(() =>
        {
            songImage.sprite = song.songImage;
            songImageMAT.DOFloat(0f, "_FadeAmount", 1f);
        });
    }
}
