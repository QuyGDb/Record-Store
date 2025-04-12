using DG.Tweening;
using EasyTextEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongDetailsUI : MonoBehaviour
{
    Image songImage;
    TextMeshProUGUI songName;
    TextEffect songNameEffect;
    private PhysicalCDAlbum PhysicalCDAlbum;
    private void Awake()
    {
        songImage = GetComponentInChildren<Image>();
        songName = GetComponentInChildren<TextMeshProUGUI>();
        songNameEffect = songName.GetComponent<TextEffect>();
        CDAlbumManager cdAlbumManager = GetComponentInParent<CDAlbumManager>();
        PhysicalCDAlbum = cdAlbumManager.albumSO.physicalCDAlbum;
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
        if (PhysicalCDAlbum == albumSO.physicalCDAlbum)
            ApplyUITransition(albumSO.songs[index]);

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
