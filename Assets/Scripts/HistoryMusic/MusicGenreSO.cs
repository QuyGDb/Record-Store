using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "MusicGenreSO", menuName = "Scriptable Objects/MusicGenreSO")]
public class MusicGenreSO : ScriptableObject
{
    public string musicGenreName;
    public GameObject[] prefabs;
    public GameObject logo;
    public VideoClip videoClip;
}
