using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VinylDisc_", menuName = "Scriptable Objects/VinylDisc", order = 1)]
public class VinylDiscSO : ScriptableObject
{
    public string discName;
    public List<AudioClip> songs;
}
