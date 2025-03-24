using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CDAlbumManager : SingletonMonobehaviour<CDAlbumManager>
{
    [SerializeField] private Image artistImage;
    [SerializeField] private TextMeshProUGUI artistName;
    [SerializeField] private TextMeshProUGUI albumName;
    [SerializeField] private TextMeshProUGUI genres;
    public AlbumSO albumSO;
    private void Start()
    {
        SelectAlbum();
    }


    private async void SelectAlbum()
    {
        switch (albumSO.physicalCDAlbum)
        {
            case PhysicalCDAlbum.Gieo:
                albumSO = GameManager.Instance.albumSOs[0];
                //wait for subcriber to be ready to receive the event
                await Awaitable.WaitForSecondsAsync(2.5f);
                break;
            case PhysicalCDAlbum.SDDBP:
                albumSO = GameManager.Instance.albumSOs[1];
                await Awaitable.WaitForSecondsAsync(2.5f);
                break;
        }
        SetAlbumInfo();
        StaticEventHandler.InvokeStartFirstSong(albumSO);

    }
    void SetAlbumInfo()
    {
        artistImage.sprite = albumSO.artistImage;
        artistName.text = "Artist: " + albumSO.artistName;
        albumName.text = "Album: " + albumSO.albumName;
        genres.text = "Genres: " + albumSO.genres;
    }
}


