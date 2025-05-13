using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlbumSO", menuName = "Scriptable Objects/AlbumSO")]
public class AlbumSO : ScriptableObject
{
    public Sprite artistImage;
    public string artistName;
    public string albumName;
    public string genres;
    public int releaseYear;
    public GameObject albumPrefab;
    public List<SongDescription> songs;
    public PhysicalCDAlbum physicalCDAlbum;


}
