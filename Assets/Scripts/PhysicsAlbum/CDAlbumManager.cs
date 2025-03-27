using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CDAlbumManager : MonoBehaviour
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

        SetAlbumInfo();
        await Awaitable.WaitForSecondsAsync(2.5f);
        StaticEventHandler.InvokeStartFirstSong(albumSO);

    }
    void SetAlbumInfo()
    {

        artistImage.sprite = albumSO.artistImage;
        artistName.text = "Artist: " + albumSO.artistName;
        albumName.text = "Album: " + albumSO.albumName;
        genres.text = "Genres: " + albumSO.genres;
        artistImage.gameObject.SetActive(false);
        albumName.gameObject.SetActive(false);
        genres.gameObject.SetActive(false);
        artistImage.gameObject.SetActive(true);
        albumName.gameObject.SetActive(true);
        genres.gameObject.SetActive(true);
    }
}


